using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Application.MatchPredictions.ScorePointCalculators;
using SportBids.Application.Services;
using SportBids.Domain;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.ScorePointCalculators;

public class PredictionScoreCalculationServiceTest
{
    private readonly IEnumerable<IPredictionScoreCalculator> _scoreCalcs = new IPredictionScoreCalculator[]
    {
        new CorrectOutcome(),
        new CorrectGoalDifference(),
        new CorrectScore()
    };

    [Fact]
    public void Calculate_Returns_5_When_Score_Matches_Exactly()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(1, 1), new Score(1, 1));

        // Assert
        Assert.Equal(5, actual);
    }

    [Fact]
    public void Calculate_Returns_3_When_Score_Difference_Matches_Exactly()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(10, 8), new Score(3, 1));

        // Assert
        Assert.Equal(3, actual);
    }

    [Fact]
    public void Calculate_Returns_1_When_Outcome_Matches()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(10, 1), new Score(4, 3));

        // Assert
        Assert.Equal(1, actual);
    }

    [Fact]
    public void Calculate_Returns_3_When_Difference_and_Outcome_Matches()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(5, 4), new Score(4, 3));

        // Assert
        Assert.Equal(3, actual);
    }

    [Fact]
    public void Calculate_Returns_0_When_Score_Difference_Reversed()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(10, 8), new Score(1, 3));

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Calculate_Returns_0_When_Nothing_Matches()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(0, 0), new Score(1, 2));

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void Calculate_Returns_0_When_Score_Reversed()
    {
        // Arrange
        var sut = new PredictionScoreCalculationService(_scoreCalcs);

        // Act
        var actual = sut.Calculate(new Score(1, 0), new Score(0, 1));

        // Assert
        Assert.Equal(0, actual);
    }
}
