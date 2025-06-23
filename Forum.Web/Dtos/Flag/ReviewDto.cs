namespace Forum.Web.Dtos.Flag
{
    public class ReviewDto
    {
        public int ReviewerId { get; set; }
        public string ReviewerNick { get; set; } = default!;
        public bool ShouldBePosted { get; set; }
        public DateTime ReviewedAt { get; set; }
    }
}
