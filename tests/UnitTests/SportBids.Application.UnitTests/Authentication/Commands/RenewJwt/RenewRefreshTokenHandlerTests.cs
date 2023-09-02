using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentResults;
using Moq;
using SportBids.Application.Authentication.Commands.RenewJwt;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.Authentication.Commands.RenewJwt;

public class RenewRefreshTokenHandlerTests
{
    private readonly IFixture _fixture = new Fixture();

    public RenewRefreshTokenHandlerTests()
    {
    }

    [Theory, AutoMoqData]
    public async Task HandleRenewRefreshTokenCommand_WhenInvalidToken_ShouldReturnUserNotFoundError(
        RenewJwtCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        RenewRefreshTokenCommandHandler sut
    )
    {
        // Arrange
        authServiceMock.Setup(service => service.FindUserByRefreshTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as AppUser);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Should().BeOfType<Result<AuthResult>>();
        result.Errors.Should().AllBeOfType<UserNotFoundError>().And.ContainSingle();

    }

    [Theory, AutoMoqData]
    public async Task HandleRenewRefreshTokenCommand_WhenTokenExpired_ShouldReturnBadRefreshTokenError(
        RenewJwtCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        RenewRefreshTokenCommandHandler sut
    )
    {
        // Arrange
        authServiceMock
            .Setup(service => service.FindUserByRefreshTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                _fixture.Build<AppUser>()
                        .With(
                            user => user.RefreshTokens,
                            new List<RefreshToken>(
                                _fixture
                                    .CreateMany<RefreshToken>(3)
                                    .Append(
                                        _fixture
                                            .Build<RefreshToken>()
                                            .With(rt => rt.Token, command.RefreshToken)
                                            .With(rt => rt.Expires, DateTimeOffset.UtcNow.AddDays(-1))
                                            .With(rt => rt.RevokedAt, null as DateTimeOffset?)
                                            .Create()
                                    )
                            )
                        )
                        .Create());

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsFailed.Should()
            .BeTrue();
        result.Should().BeOfType<Result<AuthResult>>();
        result.Errors.Should().AllBeOfType<BadRefreshToken>().And.ContainSingle();
    }

    [Theory, AutoMoqData]
    public async Task HandleRenewRefreshTokenCommand_WhenTokenRevoked_ShouldReturnBadRefreshTokenError(
        RenewJwtCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        RenewRefreshTokenCommandHandler sut
    )
    {
        // Arrange
        authServiceMock
            .Setup(service => service.FindUserByRefreshTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                _fixture.Build<AppUser>()
                        .With(
                            user => user.RefreshTokens,
                            new List<RefreshToken>(
                                _fixture
                                    .CreateMany<RefreshToken>(3)
                                    .Append(
                                        _fixture
                                            .Build<RefreshToken>()
                                            .With(rt => rt.Token, command.RefreshToken)
                                            .With(rt => rt.Expires, DateTimeOffset.UtcNow.AddDays(1))
                                            .With(rt => rt.RevokedAt, DateTimeOffset.UtcNow.AddDays(-1))
                                            .Create()
                                    )
                            )
                        )
                        .Create());

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsFailed.Should()
            .BeTrue();
        result.Should().BeOfType<Result<AuthResult>>();
        result.Errors.Should().AllBeOfType<BadRefreshToken>().And.ContainSingle();
    }

    [Theory, AutoMoqData]
    public async Task HandleRenewRefreshTokenCommand_WhenTokenRevoked_ShouldRevokeDescendant(
        RenewJwtCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        RenewRefreshTokenCommandHandler sut
    )
    {
        // Arrange
        _fixture.Customize<RefreshToken>(
            composerTransformation: composer => composer
                .OmitAutoProperties()
                .With(rt => rt.Expires, DateTimeOffset.UtcNow.AddDays(2))
                .With(rt => rt.RevokedAt, DateTimeOffset.UtcNow.AddDays(-1))
            );
        var refreshTokenList = new List<RefreshToken>
        {
            _fixture.Build<RefreshToken>().With(rt => rt.Token, command.RefreshToken).With(rt=>rt.ReplacedByToken, "token01").Create(),
            _fixture.Build<RefreshToken>().With(rt => rt.Token, "token01").With(rt=>rt.ReplacedByToken, "token02").Create(),
            _fixture.Build<RefreshToken>().With(rt => rt.Token, "token02").With(rt=>rt.ReplacedByToken, "token03")
                .With(rt => rt.RevokedAt, null as DateTimeOffset?).Create()
        };
        authServiceMock
            .Setup(service => service.FindUserByRefreshTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                _fixture.Build<AppUser>()
                        .With(user => user.RefreshTokens, refreshTokenList)
                        .Create());

        dateTimeProvider.Setup(dt => dt.UtcNow).Returns(DateTimeOffset.UtcNow);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsFailed.Should()
            .BeTrue();
        refreshTokenList.Should().AllSatisfy(rt => rt.IsRevoked.Should().BeTrue());
    }

    [Theory, AutoMoqData]
    public async Task HandleRenewRefreshTokenCommand_WhenTokenActive_ShouldCreateNewTokens(
        RenewJwtCommand command,
        [Frozen] Mock<IAuthService> authServiceMock,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        [Frozen] Mock<IJwtFactory> jwtFactory,
        RenewRefreshTokenCommandHandler sut
    )
    {
        // Arrange
        var refreshTokenList = new List<RefreshToken>
        {
            _fixture
                .Build<RefreshToken>()
                .With(rt=>rt.Token, command.RefreshToken)
                .With(rt=> rt.Expires, DateTimeOffset.UtcNow.AddDays(1))
                .With(rt => rt.RevokedAt, null as DateTimeOffset?)
                .Create()
        };
        authServiceMock
            .Setup(service => service.FindUserByRefreshTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                _fixture.Build<AppUser>()
                        .With(user => user.RefreshTokens, refreshTokenList)
                        .Create());

        AppUser savedUser = null!;
        authServiceMock.Setup(service => service.UpdateAsync(It.IsAny<AppUser>()))
            .Callback<AppUser>(user => savedUser = user);

        dateTimeProvider.Setup(dt => dt.UtcNow).Returns(DateTimeOffset.UtcNow);

        jwtFactory.SetReturnsDefault(new RefreshToken
        {
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByIp = "ip",
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Token = Guid.NewGuid().ToString()
        });

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsSuccess.Should()
            .BeTrue();
        result.Should().BeOfType<Result<AuthResult>>();
        savedUser.RefreshTokens.Should().SatisfyRespectively(
            rt1 => rt1.IsRevoked.Should().BeTrue(),
            rt2 => rt2.IsActive.Should().BeTrue());
    }
}
