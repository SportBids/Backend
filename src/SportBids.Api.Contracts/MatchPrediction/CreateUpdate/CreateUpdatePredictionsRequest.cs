#nullable disable

namespace SportBids.Api.Contracts.MatchPrediction.CreateUpdate;

public class CreateUpdatePredictionsRequest
{
    public PredictionDto[] Predictions { get; set; }
    public Guid? OwnerId { get; set; }
}
