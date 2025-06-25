using Forum.Web.Data.Entities;
using Forum.Web.Models;
using Forum.Web.Repositories.Contracts.Base;
using Forum.Web.Services.Contracts;
using Forum.Web.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Services
{
    public class AnalitycsService : IAnalyticsService
    {
        private readonly IUnitOfWork worker;

        public AnalitycsService(IUnitOfWork worker) => this.worker = worker;

        public async Task<AdminDashboardViewModel> GetAdminDashboardData(UserManager<ApplicationUser> userManager)
        {
            try
            {
                var totalUsers = await TotalUsersAsync();
                var moderatorsCount = await UsersByRoleCountAsync("Moderator", userManager);
                var adminsCount = await UsersByRoleCountAsync("Admin", userManager);
                var usersCount = totalUsers - moderatorsCount - adminsCount;// could be fetched

                return new AdminDashboardViewModel
                {
                    TotalUsers = totalUsers,
                    AdminsCount = moderatorsCount,
                    ModeratorsCount = adminsCount,
                    UsersCount = usersCount,
                };
            }
            catch (Exception ex)
            {
                // ToDo
                return null;
            }
        }

        public async Task<ModeratorDashboardViewModel> GetModeratorDashBoardData(string userId)
        {
            try
            {
                var activeUsers = await ActiveUsersCountAsync();
                var pendingReports = await PendingReportsAsync();
                var flaggedPosts = await TotalPostsCountAsync();
                var resolvedToday = await DailyResolvedFlagsAsync(userId, DateTime.Now);

                return new ModeratorDashboardViewModel
                {
                    PendingReports = pendingReports,
                    ActiveWarnings = 4, // What is the diff compared to pending repots
                    FlaggedPosts = 5, // What is the diff compared to pending repots
                    ResolvedToday = resolvedToday,
                };
            }
            catch (Exception ex)
            {
                // ToDo
                return null;
            }
        }

        public async Task<UserHomeViewModel> GetLoggedUserDashboardData()
        {
            try
            {
                var timestamp = DateTime.Now;
                var startOfDay = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 0, 0, 0);
                var endOfDay = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 23, 59, 59);

                var totalPosts = await TotalPostsCountAsync();
                var totalUsers = await TotalUsersAsync();
                var onlineUsers = await ActiveUsersCountAsync();
                var todaysPosts = await PostsForDateRangeAsync(startOfDay, endOfDay);

                return new UserHomeViewModel
                {
                    TotalPosts = totalPosts,
                    TotalUsers = totalUsers,
                    OnlineUsers = onlineUsers,
                    TodaysPosts = todaysPosts.Count,
                    LatestDiscussions = todaysPosts.ToDtos().ToList(),
                };
            }
            catch (Exception ex)
            {
                // ToDo
                return null;
            }
        }

        public async Task<GuestHomeViewModel> GetAnonymousUserDashboardData()
        {
            try
            {
                var timestamp = DateTime.Now;
                var startOfDay = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 0, 0, 0);
                var endOfDay = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 23, 59, 59);

                var totalPosts = await TotalPostsCountAsync();
                var totalUsers = await TotalUsersAsync();
                var onlineUsers = await  ActiveUsersCountAsync();
                var todaysPosts = await  PostsForDateRangeAsync(startOfDay, endOfDay);

                return new GuestHomeViewModel()
                {
                    TotalPosts = totalPosts,
                    TotalMembers = totalUsers,
                    OnlineUsers = onlineUsers,
                    LatestDiscussions = todaysPosts.ToDtos().ToList(),
                    
                };
            }
            catch (Exception ex)
            {
                // ToDo
                return null;
            }
        }

        // Admin
        private async Task<int> ActiveUsersCountAsync()
            => await worker.UserRepository.Entities.CountAsync(u => u.IsActive);

        private async ValueTask<int> UsersByRoleCountAsync(string role, UserManager<ApplicationUser> userManager)
        {
            var users = await userManager.GetUsersInRoleAsync(role);
            return users.Count;
        }

        private async Task<int> PendingReportsAsync()
            => 5; // need to add the flag entities

        private async Task<int> TotalPostsCountAsync()
            => await worker.PostRepository.Entities.CountAsync();

        private async Task<int> TotalUsersAsync()
            => await worker.UserRepository.Entities.CountAsync();

        // Moderator
        private async Task<int> DailyResolvedFlagsAsync(string userId, DateTime date)
            => 5; // Need flags repo


        // User
        private async Task<List<Post>> PostsForDateRangeAsync(DateTime dateFrom, DateTime dateTo)
            => await worker.PostRepository.Entities
                    .Where(p => p.CreatedAt >= dateFrom || p.CreatedAt <= dateTo)
                    .ToListAsync();

    }
}
