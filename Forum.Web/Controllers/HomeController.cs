using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (true || User.IsInRole("Admin"))
            {
                var model = new AdminDashboardViewModel
                {
                    ActiveUsers = 5,
                    PendingReports = 3,
                    TotalPosts = 14,
                    TotalUsers = 10
                };
                return View("Admin", model);
            }
            else if (User.IsInRole("Moderator"))
            {
                return View("Moderator");
            }
            else if(User.IsInRole("User"))
            {
                return View("User");
            }

            return View("GuestHome");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
