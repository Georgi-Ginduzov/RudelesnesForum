namespace Forum.Web.Dtos
{
    public class PostDto
    {
        public long PostId { get; set; }
        public string Content { get; set; } = default!;
        public string Creator { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
