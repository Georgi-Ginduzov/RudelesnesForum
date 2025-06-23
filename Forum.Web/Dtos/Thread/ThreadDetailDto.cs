namespace Forum.Web.Dtos.Thread
{
    public class ThreadDetailDto
    {
        public long ThreadId { get; set; }
        public string Title { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<PostDto> Posts { get; set; } = [];
        public object Topic { get; internal set; } = default!;
        public object Creator { get; internal set; } = default!;
    }
}
