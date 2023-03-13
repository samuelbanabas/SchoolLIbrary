using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;
using System.Security.Policy;

namespace SchoolLIbrary.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(UserManager<ApplicationUser> userManager, LibraryDbContext context, IWebHostEnvironment webHostEnvironment) : base(userManager)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Get user from the logged in User
                var user = await _userManager.GetUserAsync(User);

                //Check user role
                if (user.UserType == "Admin")
                {
                    var totalMaterials = _context.Materials
                        .Sum(m => m.Quantity);

                    var availableMaterials = _context.Materials
                        .Where(m => !_context.Checkouts.Any(c => c.Material.Id == m.Id && (c.Status == "CheckedOut" || c.Status == "Borrowed")))
                        .Sum(m => m.Quantity);
                    var totalStudents = _context.Users.Where(u => u.UserType == "Student").Count();
                    var totalLecturers = _context.Users.Where(u => u.UserType == "Lecturer").Count();
                    var borrowedMaterials= _context.Checkouts.Where(c=>c.Status=="Borrowed").Count(); 
                    var checkedOutMaterials = _context.Checkouts.Where(c => c.Status == "CheckOut").Count(); 
                    var returnedMaterials = _context.Checkouts.Where(c => c.Status == "Returned").Count();
                    var totalE_materials = _context.Materials
                        .Where(m => !string.IsNullOrEmpty(m.MaterialUrl))
                        .Sum(m => m.Quantity);
                    var recentMaterials = _context.Materials.OrderByDescending(m => m.DateCreated).Take(5).ToList();
                    var topBorrowedMaterials = _context.Checkouts
                        .Where(c => c.Status == "Borrowed")
                        .GroupBy(c => c.Material)
                        .Select(g => new { Material = g.Key, BorrowCount = g.Count() })
                        .OrderByDescending(g => g.BorrowCount)
                        .Take(5)
                        .Select(g => g.Material)
                        .ToList();


                    var model = new HomeViewModel
                    {
                       TotalMaterials = (int)totalMaterials!,
                       AvailableMaterials = (int)availableMaterials!,
                       TotalStudents=totalStudents,
                       BorrowedMaterials= borrowedMaterials,
                       TotalLecturers = totalLecturers,
                       CheckedOutMaterials = checkedOutMaterials,
                       ReturnedMaterials = returnedMaterials,
                       TotalE_materials= (int)totalE_materials!,
                       RecentMaterials= recentMaterials,
                       TopBorrowedMaterials=topBorrowedMaterials
                    };

                    return View(model);
                }
                else
                {
                    // User is not an Admin
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                // User is not logged in
                return RedirectToAction("Login", "Account");
            }
        }

        // -------------STUDENT ACTIONS START HERE------------------
        //================================================================

        public IActionResult StudentList(string searchString)
        {
            var students = _context.Users
                .Where(u => u.UserType == "Student")
                .Select(u => new StudentViewModel
                {
                    Id = u.Id,
                    RegNo = u.RegNo,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNo = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserType = u.UserType
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.RegNo.Contains(searchString));
            }
            var studentList = students.ToList();

            if (studentList.Count == 0)
            {
                ViewData["EmptyRecordMessage"] = "No records found.";
            }

            return View(studentList);
        }

        public async Task<IActionResult> StudentDetails(string id)
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
                UserType = user.UserType
            };

            return View(student);
        }

        public async Task<IActionResult> EditStudent(string id)
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
        public async Task<IActionResult> EditStudent(string id, StudentViewModel model)
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
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                        //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
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
                    TempData["ErorMessage"] = "Error occured. The Student's record was not updated.";
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

                TempData["SuccessMessage"] = "Student record updated successfully.";
                return RedirectToAction(nameof(StudentList));
            }

            return View(model);
        }


        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = await _context.Users.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                RegNo = student.RegNo,
                Email = student.Email,
                PhoneNo = student.PhoneNumber,
                ProfileImageUrl = student.ProfileImageUrl,
                UserType = student.UserType
            };

            return View(viewModel);
        }


        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            // Look up the LibraryUser record by its Id
            var libraryUser = _context.LibraryUsers.FirstOrDefault(u => u.UserId == id);


            if (libraryUser == null)
            {
                TempData["ErrorMessage"] = "Record not found.";
                return RedirectToAction(nameof(Index));
            }

            // Look up the ApplicationUser record by its Id
            var applicationUser = _userManager.FindByIdAsync(libraryUser.UserId).Result;

            if (applicationUser == null)
            {
                TempData["ErrorMessage"] = "User record not found.";
                return RedirectToAction(nameof(Index));
            }

            // Start a transaction
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Delete the profile image if it exists
                    if (!string.IsNullOrEmpty(applicationUser.ProfileImageUrl))
                    {
                        var profileImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/profiles", applicationUser.ProfileImageUrl);
                        if (System.IO.File.Exists(profileImagePath))
                        {
                            System.IO.File.Delete(profileImagePath);
                        }
                    }

                    // Delete the LibraryUser record
                    _context.LibraryUsers.Remove(libraryUser);

                    // Delete the ApplicationUser record
                    var result = _userManager.DeleteAsync(applicationUser).Result;

                    if (!result.Succeeded)
                    {
                        TempData["ErrorMessage"] = $"Failed to delete user {applicationUser.UserName}: {result.Errors.FirstOrDefault()?.Description}";
                        return RedirectToAction(nameof(StudentList));
                    }

                    // Commit the transaction
                    transaction.Commit();

                    TempData["SuccessMessage"] = "Record deleted successfully.";
                    return RedirectToAction(nameof(StudentList));
                }
                catch (Exception ex)
                {
                    // Roll back the transaction on error
                    transaction.Rollback();
                    TempData["ErrorMessage"] = $"An error occurred while deleting the record: {ex.Message}";
                    return RedirectToAction(nameof(StudentList));
                }
            }
        }

        // -------------BOOK ACTIONS START HERE------------------
        //================================================================

        public async Task<IActionResult> BookList(string search)
        {
            // Retrieve all books from the database
            var books = await _context.Materials.ToListAsync();

            // Filter the books by the search string
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                b.Author.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.MaterialType.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Map the book objects to the BookViewModel for display
            var bookViewModels = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                Year = b.Year,
                Genre = b.Genre,
                MaterialType = b.MaterialType,
                Falculty = b.Falculty,
                Department = b.Department,
                Quantity = b.Quantity,
                DateCreated = b.DateCreated,
                QuantityLeft = b.Quantity - _context.Checkouts
                    .Count(c => c.Material.Id == b.Id && (c.Status == "CheckedOut" || c.Status == "Borrowed")),
            });

            if (bookViewModels == null || !bookViewModels.Any())
            {
                TempData["NoFoundMessage"] = "No records found.";
            }

            // Pass the BookViewModels to the view for display
            return View(bookViewModels);
        }


        // GET: /Books/Details/5
        public async Task<IActionResult> BookDetails(int? id)
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

        // GET: /Books/Create
        public IActionResult AddBook()
        {
            // Return an empty BookViewModel to the view for display
            return View(new BookViewModel());
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the BookViewModel to a Book object for saving to the database
                var book = new MaterialModel
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
                    DateCreated = DateTime.Now
                };

                // Save the new book to the database
                _context.Materials.Add(book);
                await _context.SaveChangesAsync();

                //Display success message
                TempData["Message"] = "Book has been added successfully";

                return RedirectToAction(nameof(BookList));
            }

            // If the model state is not valid, return the BookViewModel back to the view with validation errors
            return View(model);
        }
        // GET: Book/Edit/5
        // Returns the Edit view for a specific book
        public async Task<IActionResult> EditBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the book with the specified id in the database
            var book = await _context.Materials.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Create a view model for the book data
            var bookViewModel = new BookViewModel()
            {
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre,
                MaterialType = book.MaterialType,
                Falculty = book.Falculty,
                Department = book.Department,
                Quantity = book.Quantity,
                //DateCreated = book.DateCreated
            };

            return View(bookViewModel);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(int id, BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound();
                }

                material.Title = bookViewModel.Title;
                material.Author = bookViewModel.Author;
                material.Publisher = bookViewModel.Publisher;
                material.Year = bookViewModel.Year;
                material.Genre = bookViewModel.Genre;
                material.MaterialType = bookViewModel.MaterialType;
                material.Falculty = bookViewModel.Falculty;
                material.Department = bookViewModel.Department;
                material.Quantity = bookViewModel.Quantity;

                try
                {
                    _context.Materials.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                    {
                        TempData["Message"] = "Material not found.";

                        return RedirectToAction(nameof(BookList));
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Message"] = "Material has been successfully updated.";

                return RedirectToAction(nameof(BookList));
            }

            return View(bookViewModel);
        }
        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }


        private bool BookExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> DeleteBook(int? id)
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
        [HttpPost, ActionName("DeleteBook")]
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

            return RedirectToAction(nameof(BookList));
        }

        //------BORROWING AND CHECKED OUT ACTIONS STARTS HERE--------
        //==============================================================
        public async Task<IActionResult> CheckedoutAndBorrowed(string searchString)
        {
            // Get all checked out or borrowed materials and their quantity
            var checkouts = await _context.Checkouts
                .Include(c => c.Material)
                .Include(c => c.User)
                .Where(c => (c.Status == "CheckedOut" || c.Status == "Borrowed" || c.Status == "Returned")
                    && (string.IsNullOrEmpty(searchString)
                        || c.Material.Title.ToLower().Contains(searchString.ToLower())
                        || c.User.RegNo.ToLower().Contains(searchString.ToLower())
                        || c.CheckoutCode.ToLower().Contains(searchString.ToLower())))
                .ToListAsync();

            // Create a view model to hold the checked out or borrowed materials and their details
            var viewModel = checkouts.Select(c => new CheckedOutMaterialsViewModel
            {
                Id = c.Id,
                MaterialId = c.Material.Id,
                MaterialTitle = c.Material.Title,
                MaterialType = c.Material.MaterialType,
                Fine = c.Fine.HasValue ? c.Fine.Value : decimal.Zero,
                Quantity = (int)c.Material.Quantity,
                QuantityLeft = (int)(c.Material.Quantity - _context.Checkouts
                    .Count(cc => cc.Material.Id == c.Material.Id && cc.Status == "CheckedOut" || cc.Material.Id == c.Material.Id && cc.Status == "Borrowed")),
                UserName = c.User.FirstName + " " + c.User.LastName,
                RegNo = c.User.RegNo,
                PhoneNumber = c.User.PhoneNumber,
                Status = c.Status,
                CheckoutCode = c.CheckoutCode,
                CheckoutDate= c.CheckoutDate,
                ReturnDate= c.ReturnDate
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptReturn(int checkoutId)
        {
            var checkout = await _context.Checkouts
                .Include(c => c.Material)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == checkoutId && c.Status == "Borrowed");

            if (checkout == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("CheckedoutAndBorrowed");
            }

            // Calculate fine (if any)
            var daysLate=0;
            var dueDate = checkout.ReturnDate.Date;
            var actualReturnDate = DateTime.Today;
            if (actualReturnDate > dueDate)
            {
                 daysLate = (int)(actualReturnDate - dueDate).TotalDays;
                var finePerDay = 20.0m; // $0.50 per day
                checkout.Fine = daysLate * finePerDay;                
            }

            checkout.Status = "Returned";
            await _context.SaveChangesAsync();

            if(checkout.Fine > 0)
            {
                TempData["SuccessMessage"] = "Book has been successfully returned with a fine of N" + checkout.Fine + 
                    ". The student was " + daysLate + " days late.";
            }
            else
            {
                TempData["SuccessMessage"] = "Book has been successfully returned without a fine";
            }
            return RedirectToAction("CheckedoutAndBorrowed");
        }

        [HttpPost]
        public async Task<IActionResult>ConfirmBorrowing(int checkoutId)
        {
            var checkout = await _context.Checkouts.FindAsync(checkoutId);

            if (checkout == null)
            {
                return NotFound();
            }

            checkout.Status = "Borrowed";
            checkout.CheckoutDate = DateTime.Now;
            checkout.ReturnDate = DateTime.Now.AddMonths(1);

            _context.Update(checkout);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Book has been successfully borrowed";
            return RedirectToAction(nameof(CheckedoutAndBorrowed));
        }

        public async Task<IActionResult> RegistrationCode(string searchString)
        {
            var regCodes = await _context.Regcodes
                .Where(rc => string.IsNullOrEmpty(searchString)
                    || rc.StaffNo.ToLower().Contains(searchString.ToLower())
                    || rc.Code.ToLower().Contains(searchString.ToLower()))
                .OrderByDescending(rc => rc.DateGenerated)
                .ToListAsync();

            var viewModel = regCodes.Select(rc => new RegistrationCodeViewModel
            {
                Id = rc.Id,
                StaffNo = rc.StaffNo,
                Code = rc.Code,
                DateGenerated = rc.DateGenerated
            });

            return View(viewModel);
        }

        public IActionResult GenerateCode()
        {
            // Create a new instance of the RegistrationCodeViewModel to pass to the view
            var viewModel = new RegistrationCodeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateCode(RegistrationCodeViewModel viewModel)
        {
            // Generate a new registration code
            var code2 = GenerateRegistrationCode();

        // Check if a registration code has already been generated for the staff
            if (await _context.Regcodes.AnyAsync(rc => rc.StaffNo == viewModel.StaffNo))
            {
                TempData["ErrorMessage"] = "A registration code has already been generated for this staff.";
               // ModelState.AddModelError("StaffNo", "A registration code has already been generated for this staff.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {                

                // Create a new RegistrationCodeModel and fill in the properties
                var registrationCode = new RegistrationCodeModel
                {
                    Id= viewModel.Id,
                    StaffNo = viewModel.StaffNo,
                    Code = code2,
                    DateGenerated = DateTime.Now
                };

                // Add the new registration code to the database and save changes
                _context.Regcodes.Add(registrationCode);
                await _context.SaveChangesAsync();

                // Redirect to the view that displays the registration code that was generated
                return RedirectToAction("DisplayCode", new { id = registrationCode.Id });
            }

            return View(viewModel);
        }

        private string GenerateRegistrationCode()
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

        public async Task<IActionResult> DisplayCode(int id)
        {
            // Find the registration code with the specified ID in the database
            var registrationCode = await _context.Regcodes.FindAsync(id);

            // If no registration code was found, return a 404 Not Found status code
            if (registrationCode == null)
            {
                return NotFound();
            }

            // Create a view model to hold the registration code details
            var viewModel = new RegistrationCodeViewModel
            {
                Id = registrationCode.Id,
                StaffNo = registrationCode.StaffNo,
                Code = registrationCode.Code,
                DateGenerated = registrationCode.DateGenerated
            };

            return View(viewModel);
        }


        //-----------LECTURER STARTS HERE-------------
        //========================================================

        public IActionResult LecturerList(string searchString)
        {
            var lecturers = _context.Users
                .Where(u => u.UserType == "Lecturer")
                .Select(u => new LecturerViewModel
                {
                    Id = u.Id,
                    RegNo = u.RegNo,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNo = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserType = u.UserType
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                lecturers = lecturers.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.RegNo.Contains(searchString));
            }
            var lecturerList = lecturers.ToList();

            if (lecturerList.Count == 0)
            {
                ViewData["EmptyRecordMessage"] = "No records found.";
            }

            return View(lecturerList);
        }

        public async Task<IActionResult> LecturerDetails(string id)
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
                UserType = user.UserType
            };

            return View(lecturer);
        }

        public async Task<IActionResult> DeleteLecturer(string id)
        {
            var lecturer = await _context.Users.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            var viewModel = new LecturerViewModel
            {
                Id = lecturer.Id,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                RegNo = lecturer.RegNo,
                Email = lecturer.Email,
                PhoneNo = lecturer.PhoneNumber,
                ProfileImageUrl = lecturer.ProfileImageUrl,
                UserType = lecturer.UserType
            };

            return View(viewModel);
        }


        [HttpPost, ActionName("DeleteLecturer")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLecturerConfirmed(string id)
        {
            // Look up the LibraryUser record by its Id
            var libraryUser = _context.LibraryUsers.FirstOrDefault(u => u.UserId == id);


            if (libraryUser == null)
            {
                TempData["ErrorMessage"] = "Record not found.";
                return RedirectToAction(nameof(Index));
            }

            // Look up the ApplicationUser record by its Id
            var applicationUser = _userManager.FindByIdAsync(libraryUser.UserId).Result;

            if (applicationUser == null)
            {
                TempData["ErrorMessage"] = "Lecturer record not found.";
                return RedirectToAction(nameof(Index));
            }

            // Start a transaction
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Delete the profile image if it exists
                    if (!string.IsNullOrEmpty(applicationUser.ProfileImageUrl))
                    {
                        var profileImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/profiles", applicationUser.ProfileImageUrl);
                        if (System.IO.File.Exists(profileImagePath))
                        {
                            System.IO.File.Delete(profileImagePath);
                        }
                    }

                    // Delete the LibraryUser record
                    _context.LibraryUsers.Remove(libraryUser);

                    // Delete the ApplicationUser record
                    var result = _userManager.DeleteAsync(applicationUser).Result;

                    if (!result.Succeeded)
                    {
                        TempData["ErrorMessage"] = $"Failed to delete lecturer {applicationUser.UserName}: {result.Errors.FirstOrDefault()?.Description}";
                        return RedirectToAction(nameof(StudentList));
                    }

                    // Commit the transaction
                    transaction.Commit();

                    TempData["SuccessMessage"] = "Record deleted successfully.";
                    return RedirectToAction(nameof(StudentList));
                }
                catch (Exception ex)
                {
                    // Roll back the transaction on error
                    transaction.Rollback();
                    TempData["ErrorMessage"] = $"An error occurred while deleting the record: {ex.Message}";
                    return RedirectToAction(nameof(StudentList));
                }
            }
        }
    }
}
