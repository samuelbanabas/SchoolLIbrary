using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;

namespace SchoolLIbrary.Controllers
{
    public class AccountController :  BaseController //Base controller was created to contain codes that will be executed
                                                     //before any action from the controllers that inherits it
    {
        //To use the context in a controller or other class, inject it into the constructor:
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LibraryDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryDbContext context,
            ILogger<AccountController> logger, IEmailSender emailSender): base(userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
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
                 user = new ApplicationUser { 
                    UserName = model.Username, 
                    Email = model.Email,
                    FirstName= model.FirstName,
                    LastName= model.LastName,
                    ProfileImageUrl = "/images/profiles/" + fileName,

                    PhoneNumber=model.PhoneNo,
                    RegNo = model.RegNo,
                    UserType=model.UserType
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the "User" role to the new user
                    await _userManager.AddToRoleAsync(user, "User");

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
                    return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
                
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Check if the user exists in the database
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "eee login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Check the user's role and redirect to the appropriate view
                    if (user.UserType == "Admin")
                    {                        
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.UserType == "Student")
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (user.UserType == "Lecturer")
                    {
                        return RedirectToAction("Index", "Lecturer");
                    }
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //Redisplay login form
                return RedirectToAction("Login");
            }
        }

        public IActionResult Lockout()
        {
            return View();
        }

        // Account/Logout        
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await _signInManager.SignOutAsync();

            // Redirect the user to the login page
            return RedirectToAction("Login");
        }
    }
}
