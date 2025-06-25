using Forum.Web.Configuration;
using Forum.Web.ML.Models;
using Forum.Web.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using System.Reflection;

namespace Forum.Web.Services
{
    public class ContentModerationService : IContentModerationService
    {
        private readonly ContentModerationConfiguration _configuration;

        public ContentModerationService(IOptionsMonitor<ContentModerationConfiguration> options)
        {
            _configuration = options.CurrentValue;
        }

        public bool IsRudeAsync(string text)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = assembly.Location;
            var modelPath = Path.Combine(assemblyPath, _configuration.MLModelPath);

            var mlContext = new MLContext();
            var loadedModel = mlContext.Model.Load(modelPath, out var modelSchema);
            var predictor = mlContext.Model.CreatePredictionEngine<CommentInput, CommentPrediction>(loadedModel);

            var comment = new CommentInput { comment_text = text };
            var predictionResult = predictor.Predict(comment);
            return predictionResult.ShouldBlock;
        }
    }
}
