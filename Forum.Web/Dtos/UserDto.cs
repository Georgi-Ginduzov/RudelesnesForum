using Forum.Web.Dtos.Thread;

namespace Forum.Web.Dtos
{
    public record UserDto
    {
        public int UserId { get; set; }
        public RoleDto Role { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string NickName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ThreadDto> Threads { get; set; } = [];
        public ICollection<PostDto> Posts { get; set; } = [];
        public ICollection<RudenessReviewDto> Reviews { get; set; } = [];
    }
}
