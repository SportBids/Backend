using SportBids.Domain;

namespace SportBids.Application;

public interface IPredictionScoreCalculator
{
    int Calculate(Score matchScore, Score predictionScore);
}
