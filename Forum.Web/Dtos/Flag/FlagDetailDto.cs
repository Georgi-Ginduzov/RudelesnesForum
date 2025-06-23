using Forum.Web.Data.Entities.Enums;

namespace Forum.Web.Dtos.Flag
{
    public class FlagDetailDto
    {
        public long ScanId { get; set; }
        public long PostId { get; set; }
        public string PostContent { get; set; } = default!;
        public string CreatorNick { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string RudeLevel { get; set; } = default!;
        public ScanStatus Status { get; set; }

        public IEnumerable<ReviewDto> Reviews { get; set; } = [];
    }
}
