//using Forum.Web.Data.Entities.Enums;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Forum.Web.Data.Entities
//{
//    public class RudenessScan
//    {
//        [Key]
//        public long ScanId { get; set; }

//        [ForeignKey(nameof(Post))]
//        public long PostId { get; set; }
//        public Post Post { get; set; }

//        public RudeLevel RudeLevel { get; set; }
//        public ScanStatus Status { get; set; } = ScanStatus.Pending;

//        public DateTime CreatedAt { get; set; }

//        public ICollection<RudenessReview> Reviews { get; set; }
//    }
//}
