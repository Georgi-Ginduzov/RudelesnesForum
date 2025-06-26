using Forum.Web.Data.Entities;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService service;
        private readonly UserManager<ApplicationUser> userManager;
        public HomeController(IAnalyticsService service, UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            this.userManager = userManager;
        }
            
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var model = await service.GetAdminDashboardData();
                return View("Admin", model);
            }
            else if (User.IsInRole("Moderator"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var model = await service.GetModeratorDashBoardData(userId!);
                return View("Moderator", model);
            }
            else if (User.IsInRole("User"))
            {
                var model = await service.GetLoggedUserDashboardData();
                return View("User", model);
            }
            else
            {
                var model = await service.GetAnonymousUserDashboardData();
                return View("Guest", model);
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
