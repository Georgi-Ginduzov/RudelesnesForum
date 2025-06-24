//using System.ComponentModel.DataAnnotations.Schema;

//namespace Forum.Web.Data.Entities
//{
//    public class RudenessReview
//    {
//        [ForeignKey(nameof(RudenessScan))]
//        public long RudenessScanId { get; set; }
//        public RudenessScan RudenessScan { get; set; }

//        [ForeignKey(nameof(Reviewer))]
//        public int ReviewerId { get; set; }
//        public ApplicationUser Reviewer { get; set; }

//        public bool ShouldBePosted { get; set; }
//        public DateTime ReviewedAt { get; set; }
//    }
//}
