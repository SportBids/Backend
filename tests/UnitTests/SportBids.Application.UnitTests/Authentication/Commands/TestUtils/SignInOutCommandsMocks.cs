using FluentResults;
using Moq;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Application.UnitTests.Authentication.Commands.TestUtils;

public static class SignInOutCommandsMocks
{
    public static Mock<IUserRepository> Create_Success_Mock(this Mock<IUserRepository> mock, User user, string password)
    {
        mock
            .Setup(r => r.Create(user, password))
            .ReturnsAsync(Result.Ok(new User { Id = Guid.NewGuid(), Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName }));
        return mock;
    }

    public static Mock<IUserRepository> Create_Failure_Mock(this Mock<IUserRepository> mock, User user, string password)
    {
        mock
            .Setup(r => r.Create(user, password))
            .ReturnsAsync(Result.Fail(new UserCreationError()));
        return mock;
    }

    public static Mock<IUserRepository> FindByUsername_Mock(this Mock<IUserRepository> mock, string username, User returnUser)
    {
        mock
            .Setup(repository => repository.FindByUsername(username))
            .ReturnsAsync(returnUser);
        return mock;
    }

    public static Mock<IUserRepository> GetUserIfValidPassword_Mock(this Mock<IUserRepository> mock, string userName, string password, User? returnResult)
    {
        mock
            .Setup(repository => repository.GetUserIfValidPassword(userName, password))
            .ReturnsAsync(returnResult);
        return mock;
    }

    public static Mock<IJwtFactory> GenerateTokens(this Mock<IJwtFactory> mock)
    {
        mock
            .Setup(factory => factory.GenerateAccessToken(It.IsAny<Guid>()))
            .Returns(new string('x', 55));
        mock
            .Setup(factory => factory.GenerateRefreshToken())
            .Returns(new string('X', 55));
        return mock;
    }
}
