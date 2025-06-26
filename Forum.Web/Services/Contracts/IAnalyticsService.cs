using Forum.Web.Data.Entities;
using Forum.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Web.Services.Contracts
{
    public interface IAnalyticsService
    {
        Task<AdminDashboardViewModel> GetAdminDashboardData();
        Task<GuestHomeViewModel> GetAnonymousUserDashboardData();
        Task<UserHomeViewModel> GetLoggedUserDashboardData();
        Task<ModeratorDashboardViewModel> GetModeratorDashBoardData(string userId);
    }
}
