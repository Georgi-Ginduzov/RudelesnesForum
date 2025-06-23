using Forum.Web.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Dtos.Thread
{
    public class ThreadDto
    {
        public long ThreadId { get; set; }
        public UserDto Creator { get; set; } = default!;
        public string Topic { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public ICollection<PostDto> Posts { get; set; } = [];
    }
}
