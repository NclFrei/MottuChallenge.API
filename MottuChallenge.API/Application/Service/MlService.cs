using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Service;

public class MotoData
{
    [LoadColumn(0)]
    public float AreaId { get; set; }

    [LoadColumn(1)]
    public float ModeloHash { get; set; }
}

public class MotoClusterPrediction
{
    [ColumnName("PredictedLabel")] public uint PredictedLabel { get; set; }
    [ColumnName("Score")] public float[] Score { get; set; }
}

public class MlService
{
    private readonly MLContext _mlContext;

    public MlService()
    {
        _mlContext = new MLContext(seed: 0);
    }

    public IEnumerable<(int MotoId, uint Cluster)> ClusterMotos(IEnumerable<Moto> motos, int clusters = 3)
    {
        // Prepare data
        var data = motos.Select(m => new MotoData
        {
            AreaId = m.AreaId,
            ModeloHash = HashModelo(m.Modelo)
        }).ToList();

        if (!data.Any())
            return Array.Empty<(int, uint)>();

        // Ensure the requested number of clusters is valid for the number of examples.
        // ML.NET KMeans requires at most as many clusters as training instances.
        if (clusters <= 0)
            clusters = 1;

        if (clusters > data.Count)
            clusters = data.Count;

        var idList = motos.Select(m => m.Id).ToList();

        var dataView = _mlContext.Data.LoadFromEnumerable(data);

        var options = new KMeansTrainer.Options
        {
            FeatureColumnName = "Features",
            NumberOfClusters = clusters
        };

        var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(MotoData.AreaId), nameof(MotoData.ModeloHash))
            .Append(_mlContext.Clustering.Trainers.KMeans(options));

        var model = pipeline.Fit(dataView);
        var predictor = _mlContext.Model.CreatePredictionEngine<MotoData, MotoClusterPrediction>(model);

        var results = new List<(int MotoId, uint Cluster)>();

        for (int i = 0; i < data.Count; i++)
        {
            var pred = predictor.Predict(data[i]);
            results.Add((idList[i], pred.PredictedLabel));
        }

        return results;
    }

    private static float HashModelo(string modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo)) return 0f;
        // Simple deterministic hash to transform string into a numeric feature
        unchecked
        {
            int hash = 23;
            foreach (var ch in modelo)
                hash = hash * 31 + ch;
            return Math.Abs(hash % 1000);
        }
    }
}
