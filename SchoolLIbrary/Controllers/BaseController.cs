using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolLIbrary.Models;

namespace SchoolLIbrary.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            ViewBag.Username = user?.UserName;
            ViewBag.ImagePath = user?.ProfileImageUrl;

            base.OnActionExecuting(filterContext);
        }
    }

}
