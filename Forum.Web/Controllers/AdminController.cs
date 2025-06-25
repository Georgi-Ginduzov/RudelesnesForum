using Forum.Web.Models;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdministrationService administrationService;

        public AdminController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        [HttpGet("Admin/Panel")]
        public async Task<IActionResult> Index()
        {
            var users = await administrationService.GetUsersAsync();
            var adminUsers = await administrationService.GetUsersByRoleAsync("Admin");
            var moderatorUsers = await administrationService.GetUsersByRoleAsync("Moderator");
            var normalUsers = await administrationService.GetUsersByRoleAsync("User");
            var usersViewModelList = new List<UserManagementViewModel>();
            foreach (var user in users)
            {
                var userRoles = await administrationService.GetUserRoles(user);
                var u = new UserManagementViewModel
                {
                    Id = user.Id,
                    Email = user?.Email ?? string.Empty,
                    UserName = user?.UserName ?? string.Empty,
                    Roles = userRoles.ToList()
                };
                usersViewModelList.Add(u);
            }
            var viewModel = new UserManagementPageViewModel
            {
                Users = usersViewModelList,
                TotalUsers = users.Count,
                AdminCount = adminUsers.Count,
                ModeratorCount = moderatorUsers.Count,
                RegularUserCount = normalUsers.Count
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string role)
        {
            await administrationService.AddRoleToUser(userId, role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            await administrationService.RemoveRoleFromUser(userId, role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await administrationService.DeleteUser(userId);
            return RedirectToAction("Index");
        }
    }
}
