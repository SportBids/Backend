using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Application.LeaderBoard.Commands.Create;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.PrivateLeaderBoards.Commands;

public class CreatePrivateLeaderBoardHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task HandleCreateCommand_When(
        LeaderBoardCreateCommand command,
        [Frozen] Mock<IAuthService> authService,
        LeaderBoardCreateCommandHandler sut)
    {
        // Arrange
        authService.Setup(s => s.FindById(command.OwnerId))
                   .ReturnsAsync(new AppUser() { Id = command.OwnerId });
        // Act
        var result = await sut.Handle(command, default);
        
        // Assert
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBeNull();
        result.Value.Members.Should().HaveCount(1);
        result.Value.Name.Should().BeEquivalentTo(command.Name);
        result.Value.Owner.Id.Should().Be(command.OwnerId);
        result.Value.JoinCode.Should().NotBeNull();
        result.Value.JoinCode.Length.Should().BePositive();
    }
}
