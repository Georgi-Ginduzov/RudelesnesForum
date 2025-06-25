using Forum.Web.Data;
using Forum.Web.Data.Entities;
using Forum.Web.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationService(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
            => await db.Users.ToListAsync();
        public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role)
            => await userManager.GetUsersInRoleAsync(role);

        public async Task AddRoleToUser(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return;

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }
            
        public async Task RemoveRoleFromUser(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return;

            if (await userManager.IsInRoleAsync(user, role))
                await userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
            => await userManager.GetRolesAsync(user);

        public async Task DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return;

            await userManager.DeleteAsync(user);
        }
    }
}
