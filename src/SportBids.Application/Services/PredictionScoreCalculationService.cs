using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Domain;

namespace SportBids.Application.Services;

public class PredictionScoreCalculationService : IPredictionScoreCalculationService
{
    private readonly IEnumerable<IPredictionScoreCalculator> _scoreCalculators;

    public PredictionScoreCalculationService(IEnumerable<IPredictionScoreCalculator> scoreCalculators)
    {
        _scoreCalculators = scoreCalculators;
    }

    public int Calculate(Score matchScore, Score predictionScore)
    {
        int total = 0;
        foreach (var scoreCalculator in _scoreCalculators)
        {
            total = Math.Max(total, scoreCalculator.Calculate(matchScore, predictionScore));
        }
        return total;
    }
}
