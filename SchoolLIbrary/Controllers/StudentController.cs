using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Controllers;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;
using System.Security.Claims;

public class StudentController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LibraryDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public StudentController(UserManager<ApplicationUser> userManager, LibraryDbContext context,
        SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment): base(userManager)
    {
        _userManager = userManager;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _signInManager= signInManager;
    }

    public async Task<IActionResult> Index()
    {
        // Get the current user's ID
        var user = await _userManager.GetUserAsync(User);
        var userId = user?.Id;

        if (userId == null)
        {
            return NotFound();
        }

        // Get the user's borrowed materials from the database
        var checkouts = _context.Checkouts
            .Include(c => c.Material)
            .Where(c => c.User.Id == userId && c.Status == "Borrowed" || c.User.Id == userId && c.Status == "CheckedOut" || c.User.Id == userId && c.Status == "Returned")
            .ToList();

        // Create a view model to hold the user's details and borrowed materials
        var viewModel = new StudentDashboardViewModel
        {
            User = user,
            Checkouts = checkouts,
            ProfileImageUrl = user?.ProfileImageUrl
        };

        return View(viewModel);
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

        var student = new StudentViewModel
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

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, StudentViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var student = await _userManager.FindByIdAsync(model.Id);
            if (student == null)
            {
                return NotFound();
            }

            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            student.PhoneNumber = model.PhoneNo;
            student.UserName = model.Username;
            student.RegNo = model.RegNo;
            student.Faculty = model.Faculty;
            student.Department = model.Department;
            student.UserType = model.UserType;

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
                    student.ProfileImageUrl = fileName;
                }
            }

            var result = await _userManager.UpdateAsync(student);
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
                ModelState.AddModelError(string.Empty, "A user with this RegNo. already exists.");
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
                await _userManager.AddToRoleAsync(user, "Student");

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
                return RedirectToAction("Index", "Student");
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

    //Profile Image method
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

    public async Task<IActionResult> MaterialList(string searchString)
    {
        // Get all materials and their quantity
        var materials = await _context.Materials
            .Select(m => new MaterialsViewModel
            {
                Id = m.Id,
                Title = m.Title,
                MaterialType = m.MaterialType,
                Quantity = m.Quantity,
                QuantityLeft = m.Quantity - _context.Checkouts
                    .Count(c => c.Material.Id == m.Id && (c.Status == "CheckedOut" || c.Status == "Borrowed")),
                MaterialUrl = m.MaterialUrl
            })
            .ToListAsync();

        // Filter materials based on search string
        if (!string.IsNullOrEmpty(searchString))
        {
            materials = materials.Where(m => m.Title.ToLower().Contains(searchString.ToLower())).ToList();
        }

        return View(materials);
    }

    public IActionResult Borrow(int id)
    {
        //Get the material
        var material = _context.Materials.Find(id);
        if (material == null)
        {
            return NotFound();
        }

        // Get the current user's ID
        var user = _userManager.GetUserAsync(User).Result;
        var userId = user?.Id;

        if (userId == null)
        {
            return NotFound();
        }

        // Check if the user has already borrowed this material
        var existingBorrow = _context.Checkouts
            .FirstOrDefault(c => c.Material.Id == material.Id && c.User.Id == userId && c.Status == "Borrowed");
        if (existingBorrow != null)
        {
            TempData["ErrorMessage"] = "You have already borrowed this book!";
            return RedirectToAction("MaterialList", "Student");
        }

        // Check if the user has already checked out this material
        var existingCheckout = _context.Checkouts
            .FirstOrDefault(c => c.Material.Id == material.Id && c.User.Id == userId && c.Status == "CheckedOut");
        if (existingCheckout != null)
        {
            TempData["ErrorMessage"] = "You have already checked out this book!";
            return RedirectToAction("MaterialList", "Student");
        }

        var checkoutCode = GenerateCheckoutCode();
        var checkoutViewModel = new CheckoutViewModel
        {
            MaterialId = material.Id,
            MaterialTitle = material.Title,
            CheckoutCode = checkoutCode
        };

        return View(checkoutViewModel);
    }

    private string GenerateCheckoutCode()
    {
        var code = "";
        var random = new Random();

        // Generate a 6-digit numeric code
        for (int i = 0; i < 6; i++)
        {
            code += random.Next(0, 9).ToString();
        }

        return code;
    }

    [HttpPost]
    public IActionResult ConfirmBorrow(int materialId, string code)
    {
        var material = _context.Materials.Find(materialId);
        if (material == null)
        {
            return NotFound();
        }

        var checkout = new CheckoutModel
        {
            Material = material,
            User = GetUser(), // Implement GetUser method to get the logged in user
            Status = "CheckedOut",
            CheckoutDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddMonths(1), // Add one month to the checkout date
            CheckoutCode = code
        };

        _context.Checkouts.Add(checkout);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "You have successfully Checked out the book. To complete the borrowing process, go to the library with the code!";
        return RedirectToAction("Index", "Student");
    }
    private ApplicationUser GetUser()
    {
        var userName = User.Identity.Name;
        var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
        return user!;
    }


}