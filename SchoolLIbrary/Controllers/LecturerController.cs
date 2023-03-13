using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Models.ViewModels;
using SchoolLIbrary.Models;
using SchoolLIbrary.Data.ContextClass;
using Microsoft.AspNetCore.Http;

namespace SchoolLIbrary.Controllers
{
    public class LecturerController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LecturerController(UserManager<ApplicationUser> userManager, LibraryDbContext context,
        SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment) : base(userManager)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            // Get the current user's ID
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            if (userId == null)
            {
                return NotFound();
            }

            // Get the lecturer's uploaded materials from the database
            var material = _context.Materials
                .Include(c => c.User)
                .Where(c => c.User.Id == userId &&
                    (string.IsNullOrEmpty(searchString)
                        || c.Title.ToLower().Contains(searchString.ToLower())
                        || c.Author.ToLower().Contains(searchString.ToLower())
                        || c.Publisher.ToLower().Contains(searchString.ToLower())))
                .ToList();

            // Create a view model to hold the user's details and borrowed materials
            var viewModel = new LecturerDashboardViewModel
            {
                User = user,
                Material = material,
                ProfileImageUrl = user?.ProfileImageUrl
                //SearchString = searchString
            };

            return View(viewModel);
        }


        public IActionResult RegistrationCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationCode(RegistrationCodeViewModel RegCodeModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the code exists in the database
                var registrationCode = await _context.Regcodes.FirstOrDefaultAsync(rc => rc.Code == RegCodeModel.Code);
                if (registrationCode != null)
                {
                    // Redirect to registration page
                    return RedirectToAction(nameof(Register));
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid registration code";
                }
            }
            // If the model state is not valid, return the RegistrationCodeViewModel back to the view with validation errors
            
            return View(RegCodeModel);
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check LibraryUsers Table if a user with the specified Regno already exists

                if (await _context.Users.AnyAsync(u => u.RegNo == model.RegNo))
                {
                    // If a user with the specified email already exists, display an error message
                    ModelState.AddModelError(string.Empty, "A user with this Staff No. already exists.");
                    return View(model);
                }
                // Check if a user with the specified email already exists
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // If a user with the specified email already exists, display an error message
                    ModelState.AddModelError(string.Empty, "A user with this email address already exists.");
                    return View(model);
                }

                // Check if a user with the specified username already exists
                user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    // If a user with the specified email already exists, display an error message
                    ModelState.AddModelError(string.Empty, "A user with this username already exists.");
                    return View(model);
                }

                // If the user does not exist, create a new user
                // Save the user's profile image to the server
                var fileName = SaveProfileImage(model.ProfileImage);

                // Create a new ApplicationUser object
                user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImageUrl = "/images/profiles/" + fileName,

                    PhoneNumber = model.PhoneNo,
                    RegNo = model.RegNo,
                    UserType = model.UserType
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the "User" role to the new user
                    await _userManager.AddToRoleAsync(user, "Lecturer");

                    // Add the user to the database
                    var libraryUser = new LibraryUser
                    {
                        UserId = user.Id,
                        RegNo = model.RegNo,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNo = model.PhoneNo,
                        UserType = model.UserType,
                        Password = model.Password,
                        Username = model.Username
                    };
                    _context.LibraryUsers.Add(libraryUser);
                    await _context.SaveChangesAsync();

                    //int RegResult = await _context.SaveChangesAsync();

                    // Sign the user in
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect the user to the home page
                    return RedirectToAction("Index", "Lecturer");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, "Registration attempt failed");
                    }
                }
            }

            return View(model);
        }

        private object SaveProfileImage(IFormFile image)
        {
            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            // Save the image to the wwwroot/images/profiles folder
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return fileName;
        }
                
        public IActionResult UploadMaterial()
        {
            // Return an empty MaterialUploadViewModel to the view for display
            return View(new MaterialUploadViewModel());
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMaterial(MaterialUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user
                var user = await _userManager.GetUserAsync(User);

                // Check if the uploaded file is a valid PDF or Word file
                //if (model.MaterialFile != null &&
                //    (Path.GetExtension(model.MaterialFile.FileName).ToLower() == ".pdf" ||
                //     Path.GetExtension(model.MaterialFile.FileName).ToLower() == ".doc" ||
                //     Path.GetExtension(model.MaterialFile.FileName).ToLower() == ".docx"))
                //{
                    // Save the uploaded file to the server
                    var fileName = SaveFile(model.MaterialFile);

                    // Map the MaterialUploadViewModel to a material object for saving to the database
                    var material = new MaterialModel
                    {
                        Title = model.Title,
                        Author = model.Author,
                        Publisher = model.Publisher,
                        Year = model.Year,
                        Genre = model.Genre,
                        MaterialType = model.MaterialType,
                        Falculty = model.Falculty,
                        Department = model.Department,
                        Quantity = model.Quantity,
                        DateCreated = DateTime.Now,
                        MaterialUrl = "/UploadMaterials/" + fileName,
                        User = user
                    };

                    // Save the new material to the database
                    _context.Materials.Add(material);
                    await _context.SaveChangesAsync();

                    // Display success message
                    TempData["SuccesMessage"] = "Material has been uploaded successfully";

                    return RedirectToAction(nameof(Index));
                //}

                // If the uploaded file is not a valid PDF or Word file, add an error message to ModelState
                //ModelState.AddModelError(nameof(MaterialUploadViewModel.MaterialFile), "Please upload a valid PDF or Word file");
            }

            // If the model state is not valid, return the MaterialUploadViewModel back to the view with validation errors
            return View(model);
        }

        private string SaveFile(IFormFile material)
        {
            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(material.FileName);

            // Save the file to the wwwroot/UploadMaterials folder
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadMaterials", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                material.CopyTo(fileStream);
            }

            return fileName;
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var lecturer = new LecturerViewModel
            {
                Id = user.Id,
                RegNo = user.RegNo,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNo = user.PhoneNumber,
                Faculty = user.Faculty,
                Department = user.Department,
                ProfileImageUrl = user.ProfileImageUrl,
                UserType = user.UserType,
                Username = user.UserName
            };

            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, LecturerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var lecturer = await _userManager.FindByIdAsync(model.Id);
                if (lecturer == null)
                {
                    return NotFound();
                }

                lecturer.FirstName = model.FirstName;
                lecturer.LastName = model.LastName;
                lecturer.Email = model.Email;
                lecturer.PhoneNumber = model.PhoneNo;
                lecturer.UserName = model.Username;
                lecturer.RegNo = model.RegNo;
                lecturer.Faculty = model.Faculty;
                lecturer.Department = model.Department;
                lecturer.UserType = model.UserType;

                if (model.ProfileImage != null)
                {
                    if (model.ProfileImage.Length > 0)
                    {
                        //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                        //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles", fileName);


                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ProfileImage.CopyToAsync(fileStream);
                        }
                        lecturer.ProfileImageUrl = fileName;
                    }
                }

                var result = await _userManager.UpdateAsync(lecturer);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Your record was not updated.";
                    return View(model);
                }

                var libraryUser = await _context.LibraryUsers.FirstOrDefaultAsync(l => l.UserId == model.Id);
                if (libraryUser == null)
                {
                    return NotFound();
                }

                libraryUser.FirstName = model.FirstName;
                libraryUser.LastName = model.LastName;
                libraryUser.Email = model.Email;
                libraryUser.PhoneNo = model.PhoneNo;
                libraryUser.RegNo = model.RegNo;

                _context.Update(libraryUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your record updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> MaterialDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the book with the given ID from the database
            var book = await _context.Materials.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Map the book object to the BookViewModel for display
            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre,
                MaterialType = book.MaterialType,
                Falculty = book.Falculty,
                Department = book.Department,
                Quantity = book.Quantity,
                DateCreated = book.DateCreated
            };

            // Pass the BookViewModel to the view for display
            return View(bookViewModel);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> DeleteMaterial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel
            {
                Id = material.Id,
                Title = material.Title,
                Author = material.Author,
                Publisher = material.Publisher,
                Year = material.Year,
                Genre = material.Genre,
                MaterialType = material.MaterialType,
                Falculty = material.Falculty,
                Department = material.Department,
                Quantity = material.Quantity,
                DateCreated = material.DateCreated
            };

            return View(bookViewModel);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("DeleteMaterial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Material has been successfully deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
