using Forum.Web.Data.Entities;

namespace Forum.Web.Services.Contracts
{
    public interface IAdministrationService
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task AddRoleToUser(string userId, string role);
        Task RemoveRoleFromUser(string userId, string role);
        Task DeleteUser(string userId);
    }
}
