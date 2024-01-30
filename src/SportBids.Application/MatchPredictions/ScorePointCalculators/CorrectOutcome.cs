using SportBids.Domain;

namespace SportBids.Application.MatchPredictions.ScorePointCalculators;

public class CorrectOutcome : IPredictionScoreCalculator
{
    public int Calculate(Score matchScore, Score predictionScore)
    {
        if (matchScore.Team1 > matchScore.Team2 && predictionScore.Team1 > predictionScore.Team2)
            return 1;

        if (matchScore.Team1 < matchScore.Team2 && predictionScore.Team1 < predictionScore.Team2)
            return 1;

        return 0;
    }
}
