using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Application.LeaderBoard.Commands.Update;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.PrivateLeaderBoards.Commands;

public class UpdatePrivateLeaderBoardHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task HandleUpdateCommand_WhenNotOwner_Return_NotFound(
        LeaderBoardUpdateCommand command,
        [Frozen] Mock<IPrivateLeaderBoardRepository> plbRepositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        LeaderBoardUpdateCommandHandler sut)
    {
        // Arrange
        unitOfWorkMock
            .Setup(m => m.PrivateLeaderBoards)
            .Returns(plbRepositoryMock.Object);
        plbRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(),
                                       It.IsAny<Guid>(),
                                       It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(PrivateLeaderBoard));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        plbRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(),
                                                           It.IsAny<Guid>(),
                                                           It.IsAny<CancellationToken>()),
                                Times.Once);
        result.IsSuccess
            .Should()
            .BeFalse();
        result.Errors
            .Should()
            .ContainSingle(x => x.GetType().Name == nameof(PrivateLeaderBoardNotFoundError));
    }
}
