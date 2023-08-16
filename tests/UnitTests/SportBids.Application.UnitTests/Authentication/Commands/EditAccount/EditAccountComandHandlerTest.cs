using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using MapsterMapper;
using Moq;
using SportBids.Application.Accounts.Commands.EditAccount;
using SportBids.Application.Accounts.Commands.UpdateAccount;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Application.UnitTests.Authentication.Commands.EditAccount;

public class EditAccountComandHandlerTest
{

    [Theory, AutoMoqData]
    public async Task HandleEditAccountCommand_WhenCommandIsValid_ShouldReturnOk(
        EditAccountCommand command,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        [Frozen] Mock<IMapper> mapperMock,
        EditAccountCommandHandler sut
    )
    {
        // Arrange
        User modifiedUser = new User();
        User foundUser = new User { Id = command.UserId };
        userRepositoryMock
            .Setup(x => x.FindById(command.UserId))
            .ReturnsAsync(foundUser);
        userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(u => modifiedUser = u);
        mapperMock
            .Setup(x => x.Map<EditAccountCommand, User>(command, foundUser))
            .Returns(() =>
            {
                foundUser.LastName = command.LastName;
                foundUser.FirstName = command.FirstName;
                return foundUser;
            }).Verifiable(Times.Once);

        // Act
        var response = await sut.Handle(command, default);

        // Assert
        userRepositoryMock
            .Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
        modifiedUser.Id.Should().Be(command.UserId);
        modifiedUser.FirstName.Should().Be(command.FirstName);
        modifiedUser.LastName.Should().Be(command.LastName);
        mapperMock.Verify();
    }

    [Theory, AutoMoqData]
    public async Task HandleEditAccountCommand_WhenCommandIsValid_ShouldReturnUserNotFound(
        EditAccountCommand command,
        [Frozen] Mock<IUserRepository> userRepositoryMock,
        EditAccountCommandHandler sut
    )
    {
        // Arrange
        User foundUser = null!;
        userRepositoryMock
            .Setup(x => x.FindById(command.UserId))
            .ReturnsAsync(foundUser);

        // Act
        var response = await sut.Handle(command, default);

        // Assert
        userRepositoryMock
            .Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().HaveCount(1)
            .And.AllBeOfType<UserNotFoundError>();
    }
}
