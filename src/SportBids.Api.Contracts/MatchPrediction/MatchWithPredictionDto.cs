#nullable disable

using SportBids.Api.Contracts.Tournament.GetTournament;
using SportBids.Api.Contracts.Tournament.GetTournaments;

namespace SportBids.Api.Contracts.MatchPrediction;

public class MatchWithPredictionDto
{
    public TournamentDto Tournament { get; set; }
    public MatchDto Match { get; set; }
    public PredictionDto Prediction { get; set; }
}
