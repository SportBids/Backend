#nullable disable

namespace SportBids.Api.Contracts.MatchPrediction.Get;

public class GetPredictionsResponse
{
    public IEnumerable<PredictionDto> Predictions { get; set; }
}
