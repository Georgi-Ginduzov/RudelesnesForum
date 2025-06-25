using Microsoft.ML.Data;

namespace Forum.Web.ML.Models
{
    public class CommentInput
    {
        [LoadColumn(0)] public string comment_text { get; set; }
        [LoadColumn(1)] public bool Label { get; set; } // True = Block, False = Publish
    }
}
