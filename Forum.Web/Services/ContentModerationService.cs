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
        private PredictionEngine<CommentInput, CommentPrediction>? _predictionEngine;

        public ContentModerationService(IOptionsMonitor<ContentModerationConfiguration> options)
        {
            _configuration = options.CurrentValue;
            InitializePredictor();
        }

        public bool IsRudeAsync(string text)
        {
            var comment = new CommentInput { comment_text = text };
            var predictionResult = _predictionEngine?.Predict(comment);
            return predictionResult?.ShouldBlock ?? false;
        }

        private void InitializePredictor()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = assembly.Location;
            var modelPath = Path.Combine(assemblyPath, _configuration.MLModelPath);

            var mlContext = new MLContext();
            var loadedModel = mlContext.Model.Load(modelPath, out var modelSchema);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<CommentInput, CommentPrediction>(loadedModel);
        }
    }
}
