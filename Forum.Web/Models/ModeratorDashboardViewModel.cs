namespace Forum.Web.Models
{
    public class ModeratorDashboardViewModel
    {
        public int PendingReports { get; set; }
        public int FlaggedPosts { get; set; }
        public int ResolvedToday { get; set; }
        public int ActiveWarnings { get; set; }
    }
}
