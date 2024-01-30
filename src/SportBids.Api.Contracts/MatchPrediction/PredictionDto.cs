#nullable disable

using SportBids.Api.Contracts.Tournament.GetTournament;

namespace SportBids.Api.Contracts.MatchPrediction;

public class PredictionDto
{
    public Guid MatchId { get; set; }
    public ScoreDto Score { get; set; }
    public int Points { get; set; }
}
