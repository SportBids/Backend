using AutoFixture;
using MapsterMapper;
using MediatR;
using Moq;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Application.UnitTests.Authentication.TestUtils;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.Authentication.Commands.SignUp;

public class SignUpCommandHandlerTests
{
    private readonly SignUpCommandHandler _handler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IJwtFactory> _mockJwtFactory;
    private readonly Mock<ISender> _mockSender;
    private readonly IFixture _fixture = new Fixture();

    public SignUpCommandHandlerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockMapper = new Mock<IMapper>();
        _mockJwtFactory = new Mock<IJwtFactory>();
        _mockSender = new Mock<ISender>();
        _handler = new SignUpCommandHandler(_mockJwtFactory.Object, _mockAuthService.Object, _mockMapper.Object, _mockSender.Object);
    }

    [Fact]
    public async Task HandleSignUpCommand_WhenComandIsValid_ShouldCreateUserAndReturnJwt()
    {
        // Arrange
        var command = _fixture.Create<SignUpCommand>();
        var user = new AppUser() { UserName = command.UserName, Id = Guid.NewGuid() };

        _mockMapper.Setup(mapper => mapper.Map<AppUser>(command)).Returns(user);
        _mockAuthService.Create_Success_Mock(user, command.Password);
        _mockJwtFactory.GenerateTokens();
        _mockMapper.Setup(m => m.Map<AuthResult>(It.IsAny<AppUser>()))
            .Returns(new AuthResult { FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, Email = user.Email! });

        // Act
        var result = await _handler.Handle(command, default);

        // Assert 
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.AccessToken.Length > 50);
        Assert.True(result.Value.RefreshToken.Length > 20);
    }

    [Fact]
    public async Task HandleSignUpCommand_WhenFailedCreateUser_ShouldReturnUserCreationError()
    {
        // Arrange
        var command = _fixture.Create<SignUpCommand>();
        var user = new AppUser() { UserName = command.UserName, Id = Guid.NewGuid() };

        _mockMapper.Setup(mapper => mapper.Map<AppUser>(command)).Returns(user);
        _mockAuthService.Create_Failure_Mock(user, command.Password);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert 
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<UserCreationError>());

        _mockJwtFactory.Verify(factory => factory.GenerateRefreshToken(It.IsAny<string>()), Times.Never);
    }
}
