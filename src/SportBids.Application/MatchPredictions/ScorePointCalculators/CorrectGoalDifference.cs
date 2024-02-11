using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Domain.Entities;

namespace SportBids.Application.MatchPredictions.ScorePointCalculators;

public class CorrectGoalDifference : IPredictionScoreCalculator
{
    public int Calculate(Score matchScore, Score predictionScore)
    {
        if (matchScore.Team1 - matchScore.Team2 == predictionScore.Team1 - predictionScore.Team2)
            return 3;

        return 0;
    }
}
