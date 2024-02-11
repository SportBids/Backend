using SportBids.Application.Interfaces.PredictionScorePoints;
using SportBids.Application.MatchPredictions.ScorePointCalculators;
using SportBids.Domain;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.ScorePointCalculators;

public class CorrectGoalDifferenceTests
{
    private readonly IPredictionScoreCalculator _unitUnderTest = new CorrectGoalDifference();

    [Theory]
    [MemberData(nameof(CorrectTestData))]
    public void Calculate_When_CorrectOutcome_Returns_3(Score matchScore, Score predictionScore)
    {
        var actual = _unitUnderTest.Calculate(matchScore, predictionScore);

        Assert.Equal(3, actual);
    }

    [Theory]
    [MemberData(nameof(NotCorrectTestData))]
    public void Calculate_When_NotCorrectOutcome_Returns_0(Score matchScore, Score predictionScore)
    {
        var actual = _unitUnderTest.Calculate(matchScore, predictionScore);

        Assert.Equal(0, actual);
    }

    public static IEnumerable<object[]> CorrectTestData()
    {
        var i = 3;
        while (i > 0)
        {
            var a = Random.Shared.Next(10);
            var b = Random.Shared.Next(10);
            var c = Random.Shared.Next(10);
            var d = Random.Shared.Next(10);
            if (a - b == c - d || b - a == d - c)
            {
                yield return new object[] { new Score(a, b), new Score(c, d) };
                i--;
            }
            else
                continue;
        }
    }

    public static IEnumerable<object[]> NotCorrectTestData()
    {
        var i = 3;
        while (i > 0)
        {
            var a = Random.Shared.Next(10);
            var b = Random.Shared.Next(10);
            var c = Random.Shared.Next(10);
            var d = Random.Shared.Next(10);
            if (a - b == c - d || b - a == d - c) continue;
            yield return new object[] { new Score(a, b), new Score(c, d) };
            i--;
        }
    }
}
