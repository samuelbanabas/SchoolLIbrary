using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;

namespace SchoolLIbrary.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, LibraryDbContext context) : base(userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task <IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Get user from the logged in User
                var user = await _userManager.GetUserAsync(User);

                //retrieve the user's first and last name from the LibraryUser table
                var libraryUser = _context.LibraryUsers.FirstOrDefault(x => x.UserId == user.Id);

               
                var model = new HomeViewModel
                {
                    //FirstName = libraryUser.FirstName,
                    //LastName = libraryUser.LastName
                };
               
                return View(model);
            }
            else
            {
                // User is not logged in
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}