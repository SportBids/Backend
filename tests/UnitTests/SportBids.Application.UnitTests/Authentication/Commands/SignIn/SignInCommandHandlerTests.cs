using MapsterMapper;
using Moq;
using SportBids.Application.Authentication.Commands.SignIn;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.UnitTests.Authentication.Commands.TestUtils;
using SportBids.Domain.Models;

namespace SportBids.Application.UnitTests.Authentication.Commands.SignIn;

public class SignInCommandHandlerTests
{
    private readonly SignInCommandHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtFactory> _mockJwtFactory;
    private readonly Mock<IMapper> _mockMapper;

    public SignInCommandHandlerTests()
    {
        _mockJwtFactory = new Mock<IJwtFactory>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new SignInCommandHandler(_mockUserRepository.Object, _mockJwtFactory.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task HandleSignInCommand_WhenUserNotExist_ShouldReturnSignInError()
    {
        // Arrange
        var command = CreateSignInCommandUtil.CreateCommand();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsAssignableFrom<SignInError>(result.Errors.FirstOrDefault());
        Assert.Equal("Wrong username or password", result.Errors.FirstOrDefault()?.Message);
        _mockJwtFactory.Verify(factory => factory.GenerateAccessToken(It.IsAny<Guid>()), Times.Never);
        _mockJwtFactory.Verify(factory => factory.GenerateRefreshToken(), Times.Never);
    }

    [Fact]
    public async Task HandleSignInCommand_WhenBadPassword_ShouldReturnSignInError()
    {
        // Arrange
        var user = null as User;
        var command = CreateSignInCommandUtil.CreateCommand();

        _mockUserRepository
            .GetUserIfValidPassword_Mock(command.UserName, command.Password, user);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsAssignableFrom<SignInError>(result.Errors.FirstOrDefault());
        Assert.Equal("Wrong username or password", result.Errors.FirstOrDefault()?.Message);
        _mockJwtFactory.Verify(factory => factory.GenerateAccessToken(It.IsAny<Guid>()), Times.Never);
        _mockJwtFactory.Verify(factory => factory.GenerateRefreshToken(), Times.Never);
    }
}
