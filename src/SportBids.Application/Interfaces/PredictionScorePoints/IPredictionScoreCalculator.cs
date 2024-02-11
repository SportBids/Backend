using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.PredictionScorePoints;

public interface IPredictionScoreCalculator
{
    int Calculate(Score matchScore, Score predictionScore);
}
