using FluentResults;
using Moq;
using SportBids.Application.Accounts.Commands.ChangePassword;
using FluentAssertions;
using SportBids.Application.Common.Errors;
using AutoFixture.Xunit2;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.Authentication.Commands.ChangePassword;

public class ChangePasswordCommandHandlerTest
{
    [Theory, AutoMoqData]
    public async Task HandleChangePasswordCommand_WhenComandIsValid_ShouldReturnOkResult(
        ChangePasswordCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        ChangePasswordCommandHandler sut
    )
    {
        // Arrange
        authServiceMock
            .Setup(x => x.UpdatePasswordAsync(command.UserId, command.CurrentPassword, command.NewPassword))
            .ReturnsAsync(Result.Ok());
        authServiceMock
            .Setup(x => x.FindById(command.UserId))
            .ReturnsAsync(new AppUser { Id = command.UserId });

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsSuccess.Should().Be(true);
        authServiceMock
            .Verify(x => x.FindById(command.UserId), Times.Once);
        authServiceMock
            .Verify(x => x.UpdatePasswordAsync(command.UserId, command.CurrentPassword, command.NewPassword), Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task HandleChangePasswordCommand_WhenUnknownUser_ShouldReturnUserNotFoundError(
        ChangePasswordCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        ChangePasswordCommandHandler sut
    )
    {
        // Arrange
        authServiceMock
            .Setup(x => x.FindById(command.UserId))
            .ReturnsAsync(null as AppUser);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsSuccess.Should().Be(false);
        result.Errors.Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainItemsAssignableTo<UserNotFoundError>();
        authServiceMock
            .Verify(x => x.FindById(command.UserId), Times.Once);
        authServiceMock
            .Verify(x => x.UpdatePasswordAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}

