namespace Forum.Web.Dtos.Thread
{
    public class ThreadSummaryDto
    {
        public long ThreadId { get; set; }
        public string Title { get; set; } = default!;
        public int PostCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
