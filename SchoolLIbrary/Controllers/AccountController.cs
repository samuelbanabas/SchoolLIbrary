using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;

namespace SchoolLIbrary.Controllers
{    
    public class AccountController : Controller
    {
        //To use the context in a controller or other class, inject it into the constructor:
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LibraryDbContext _context;
        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            LibraryDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Register()
        {
            return View("Register");
        }
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Username, 
                    Email = model.Email,
                    FirstName= model.FirstName,
                    LastName= model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add the user to the database
                    var libraryUser = new LibraryUser
                    {           
                        UserId = user.Id,
                        RegNo= model.RegNo,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email= model.Email,
                        PhoneNo= model.PhoneNo,
                        UserType= model.UserType
                    };
                    _context.LibraryUsers.Add(libraryUser);
                    await _context.SaveChangesAsync();

                    // Sign the user in
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect the user to the home page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View("Login");
        }
    }
}
