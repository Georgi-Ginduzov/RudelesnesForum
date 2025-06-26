namespace Forum.Web.Models
{
    public class ModeratorDashboardViewModel
    {
        public int PendingReports { get; set; }
        public int MonthlyPostCount { get; set; }
        public int ResolvedToday { get; set; }
        public int Moderators { get; set; }
    }
}
