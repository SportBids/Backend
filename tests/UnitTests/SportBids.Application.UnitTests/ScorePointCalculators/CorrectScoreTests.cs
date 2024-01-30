using System.Runtime.Versioning;
using SportBids.Application.MatchPredictions.ScorePointCalculators;
using SportBids.Domain;

namespace SportBids.Application.UnitTests.ScorePointCalculators;

public class CorrectScoreTests
{
    private readonly IPredictionScoreCalculator _unitUnderTest = new CorrectScore();

    [Theory]
    [MemberData(nameof(CorrectTestData))]
    public void Calculate_When_CorrectScore_Returns_5(Score matchScore, Score predictionScore)
    {
        var actual = _unitUnderTest.Calculate(matchScore, predictionScore);

        Assert.Equal(5, actual);
    }

    [Theory]
    [MemberData(nameof(NotCorrectTestData))]
    public void Calculate_When_NotCorrectScore_Returns_0(Score matchScore, Score predictionScore)
    {
        var actual = _unitUnderTest.Calculate(matchScore, predictionScore);

        Assert.Equal(0, actual);
    }

    public static IEnumerable<object[]> CorrectTestData()
    {
        for (var i = 0; i < 3; i++)
        {
            var team1 = Random.Shared.Next(10);
            var team2 = Random.Shared.Next(10);
            yield return new object[] { new Score(team1, team2), new Score(team1, team2) };
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
            if (a == c && b == d) continue;
            yield return new object[] { new Score(a, b), new Score(c, d) };
            i--;
        }
    }
}
