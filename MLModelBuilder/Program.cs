using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using Forum.Web.ML.Models;

namespace ToxicCommentClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            TestModel();
            //TrainAndExportModel();
        }

        private static void TestModel()
        {
            var mlContext = new MLContext();

            var modelPath = "D:\\Temp\\ML\\ToxicCommentModelImproved.zip";
            ITransformer loadedModel = mlContext.Model.Load(modelPath, out var modelSchema);

            var predictor = mlContext.Model.CreatePredictionEngine<CommentInput, CommentPrediction>(loadedModel);

            var testComments = new[]
            {
                "You are brilliant and kind!",
                "You're a disgusting, horrible human being.",
                "I totally disagree with your opinion.",
                "Shut up, idiot.",
                "Let's discuss this peacefully.",
                "You're a filthy animal.",

                "Get out of here, nobody wants you around.",
                "You're such a loser.",
                "I hope you get what's coming to you.",
                "That’s the dumbest thing I’ve ever heard.",
                "Thanks so much for your contribution!",
                "Please review the guidelines before posting.",
                "You’re unbelievable. Just wow.",
                "This is why people hate you.",
                "Calm down, and maybe read what was written.",
                "You're the reason everything is broken.",
                "Great point! I hadn't thought of that.",
                "I’ll find you. Mark my words.",
                "That’s an idiotic take.",
                "Have a great weekend!",
                "I will Fuck you",
                "Hate is not tolerated",
                "You are a fat animal"
            };


            Console.WriteLine("🔍 Toxicity Prediction Results:\n");

            foreach (var comment in testComments)
            {
                var input = new CommentInput { comment_text = comment };
                var result = predictor.Predict(input);

                Console.WriteLine($"Comment: {comment}");
                Console.WriteLine($"Should Block: {result.ShouldBlock}, Probability: {result.Probability:P2}\n");
            }
        }

        private static void TrainAndExportModel()
        {
            string csvPath = "D:\\Temp\\ML\\fixed_cleaned_comments_test.csv";

            var mlContext = new MLContext();

            var records = LoadCsv(csvPath);

            IDataView trainingData = mlContext.Data.LoadFromEnumerable(records);

            var pipeline = mlContext.Transforms.Text.NormalizeText("NormalizedText", nameof(CommentInput.comment_text))
                .Append(mlContext.Transforms.Text.TokenizeIntoWords("Tokens", "NormalizedText"))
                .Append(mlContext.Transforms.Text.ApplyWordEmbedding(
                    outputColumnName: "Features",
                    inputColumnName: "Tokens",
                    modelKind: WordEmbeddingEstimator.PretrainedModelKind.GloVe50D))
                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label"));

            var model = pipeline.Fit(trainingData);

            var modelPath = "D:\\Temp\\ML\\ToxicCommentModelImproved.zip";
            mlContext.Model.Save(model, trainingData.Schema, modelPath);

            Console.WriteLine($"✅ Model trained and saved to: {modelPath}");
        }

        static List<CommentInput> LoadCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            });

            return csv.GetRecords<CommentInput>().ToList();
        }
    }
}
