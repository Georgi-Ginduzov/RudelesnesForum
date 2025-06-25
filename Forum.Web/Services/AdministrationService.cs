using Forum.Web.Data.Entities;
using Forum.Web.Repositories.Contracts.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class AdministrationService
    {
        private readonly IUnitOfWork worker;

        public AdministrationService(IUnitOfWork worker)
        {
            this.worker = worker;
        }

        public async Task<List<ApplicationUser>> GetUsers()
            => await worker.UserRepository.Entities.ToListAsync();
        public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role, UserManager<ApplicationUser> userManager)
            => await userManager.GetUsersInRoleAsync(role);
        public async Task<IdentityResult> AddRoleToUser(ApplicationUser user, string role, UserManager<ApplicationUser> userManager)
            => await userManager.AddToRoleAsync(user, role);
        public async Task<IdentityResult> RemoveRoleFromUser(ApplicationUser user, string role, UserManager<ApplicationUser> userManager)
            => await userManager.RemoveFromRoleAsync(user, role);

    }
}
