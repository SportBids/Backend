#nullable disable

namespace SportBids.Api.Contracts.MatchPrediction.Get;

public class GetPredictionsRequest
{
    public Guid[] MatchIds { get; set; }
    public Guid? OwnerId { get; set; }
}
