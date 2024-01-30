using Microsoft.EntityFrameworkCore;
using SportBids.Infrastructure.Persistence;

namespace SportBids.Infrastructure.UnitTests;

public class PredictionRepositoryTests
{
    [Fact]
    public async Task GetMatchesWithPrediction_When_NoPredictionsFound_Should_ReturnEmptyAsync()
    {
        // Arrange
        var context = ContextUtils.CreateAppDbContext();
        ContextUtils.CreateTournament(context);
        var prediction = ContextUtils.CreatePrediction(context);

        var sut = new PredictionRepository(context);
        var predictionFromDb = (await sut.GetOwnPredictionsByMatchIdAsync(new[] { prediction.MatchId }, prediction.OwnerId, CancellationToken.None))
            .First();

        // Act
        var actual = await sut.GetMatchesWithPredictionAsync(predictionFromDb.OwnerId, CancellationToken.None);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(prediction.OwnerId, actual.First().Prediction.OwnerId);
        Assert.Equal(prediction.MatchId, actual.First().Match.Id);
    }
}
