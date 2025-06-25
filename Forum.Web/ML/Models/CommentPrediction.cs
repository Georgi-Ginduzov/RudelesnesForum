using Microsoft.ML.Data;

namespace Forum.Web.ML.Models
{
    public class CommentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool ShouldBlock { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
