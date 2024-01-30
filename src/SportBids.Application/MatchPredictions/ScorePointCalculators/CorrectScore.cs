using SportBids.Domain;

namespace SportBids.Application.MatchPredictions.ScorePointCalculators;

public class CorrectScore : IPredictionScoreCalculator
{
    public int Calculate(Score matchScore, Score predictionScore)
    {
        if (matchScore.Team1 == predictionScore.Team1 && matchScore.Team2 == predictionScore.Team2)
            return 5;

        return 0;
    }
}
