using Forum.Web.Data.Entities;
using Forum.Web.Models;
using Forum.Web.Repositories.Contracts.Base;
using Forum.Web.Services.Contracts;
using Forum.Web.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Forum.Web.Services
{
    public class AnalitycsService : IAnalyticsService
    {
        private readonly IUnitOfWork worker;
        private readonly UserManager<ApplicationUser> userManager;

        public AnalitycsService(IUnitOfWork worker, UserManager<ApplicationUser> userManager)
        {
            this.worker = worker;
            this.userManager = userManager;
        }

        public async Task<AdminDashboardViewModel> GetAdminDashboardData()
        {
            try
            {
                var totalUsers = await TotalUsersAsync();
                var moderatorsCount = await UsersByRoleCountAsync("Moderator", userManager);
                var adminsCount = await UsersByRoleCountAsync("Admin", userManager);
                var usersCount = totalUsers - moderatorsCount - adminsCount;// could be fetched

                return new AdminDashboardViewModel
                {
                    TotalUsersCount = totalUsers,
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
                var monthlyPostCount = await PostsForCurrentMonthAsync();
                var pendingReports = await PendingReportsAsync(userId);
                var moderators = await GetModeratorsAsync();
                var resolvedToday = await DailyResolvedFlagsAsync(userId, DateTime.Now);

                return new ModeratorDashboardViewModel
                {
                    PendingReports = pendingReports,
                    Moderators = moderators.Count, 
                    MonthlyPostCount = monthlyPostCount,
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
                var startOfMonth = new DateTime(timestamp.Year, timestamp.Month, 1, 0, 0, 0);
                var endOfMonth = new DateTime(timestamp.Year, timestamp.Month, DateTime.DaysInMonth(timestamp.Year, timestamp.Month), 23, 59, 59);
                
                var totalPosts = await TotalPostsCountAsync();
                var totalUsers = await TotalUsersAsync();
                var currentMonthPosts = await PostsForDateRangeAsync(startOfMonth, endOfMonth);
                var todaysPosts = await PostsForDateRangeAsync(startOfDay, endOfDay);
                var pendingApproval = await PostsAwaitingApprovalAsync();

                return new UserHomeViewModel
                {
                    TotalPosts = totalPosts,
                    TotalUsers = totalUsers,
                    OnlineUsers = currentMonthPosts.Count,
                    TodaysPosts = pendingApproval,
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
                var onlineUsers = await  PostsForCurrentMonthAsync();
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
        private async Task<int> PostsForCurrentMonthAsync()
            => await worker.PostRepository.Entities.CountAsync(p => p.CreatedAt.Month == DateTime.Today.Month && p.CreatedAt.Year == DateTime.Today.Year);

        private async ValueTask<int> UsersByRoleCountAsync(string role, UserManager<ApplicationUser> userManager)
        {
            var users = await userManager.GetUsersInRoleAsync(role);
            return users.Count;
        }

        private async Task<int> PendingReportsAsync(string userId)
                => await worker.ReplyRepository.Entities
                            .CountAsync(r => r.IsFlagged && !r.IsReviewed);

        private async Task<int> TotalPostsCountAsync()
            => await worker.PostRepository.Entities.CountAsync();
        private async Task<int> TotalFlaggedRepliesCountAsync()
            => await worker.ReplyRepository.Entities.CountAsync(r => r.IsReviewed);
        private async Task<int> TotalUsersAsync()
            => await worker.UserRepository.Entities.CountAsync();

        // Moderator
        private async Task<int> DailyResolvedFlagsAsync(string userId, DateTime date)
        {
            var from = new DateTime(date.Year, date.Month, date.Day);
            var to = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            return await worker.ReplyRepository.Entities.CountAsync(r => r.UpdatedAt >= from && r.UpdatedAt <= to && r.IsReviewed);
        }


        // User
        private async Task<List<Post>> PostsForDateRangeAsync(DateTime dateFrom, DateTime dateTo)
            => await worker.PostRepository.Entities
                    .Where(p => p.CreatedAt >= dateFrom && p.CreatedAt <= dateTo)
                    .Include(p => p.User)
                    .Include(p => p.Replies)
                    .ToListAsync();
        private async Task<IList<ApplicationUser>> GetModeratorsAsync()
            => await userManager.GetUsersInRoleAsync("Moderator");

        private async Task<int> PostsAwaitingApprovalAsync()
            => await worker.ReplyRepository.Entities.CountAsync(r => !r.IsReviewed);
    }
}
