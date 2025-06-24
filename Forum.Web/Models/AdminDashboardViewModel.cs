namespace Forum.Web.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int PendingReports { get; set; }
        public int ActiveUsers { get; set; }
    }
}
