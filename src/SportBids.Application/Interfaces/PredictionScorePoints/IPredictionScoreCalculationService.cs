using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.PredictionScorePoints;

public interface IPredictionScoreCalculationService
{
    int Calculate(Score matchScore, Score predictionScore);
}
