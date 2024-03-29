﻿using System.Security.Claims;
using AutoFixture;
using MapsterMapper;
using Moq;
using SportBids.Application.Authentication.Queries.SignIn;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Application.UnitTests.Authentication.TestUtils;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.Authentication.Queries.SignIn;

public class SignInCommandHandlerTests
{
    private readonly SignInCommandHandler _handler;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IJwtFactory> _mockJwtFactory;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IFixture _fixture = new Fixture();

    public SignInCommandHandlerTests()
    {
        _mockJwtFactory = new Mock<IJwtFactory>();
        _mockAuthService = new Mock<IAuthService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new SignInCommandHandler(_mockAuthService.Object, _mockJwtFactory.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task HandleSignInCommand_WhenUserNotExist_ShouldReturnSignInError()
    {
        // Arrange
        var command = _fixture.Create<SignInCommand>();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsAssignableFrom<SignInError>(result.Errors.FirstOrDefault());
        Assert.Equal("Wrong username or password", result.Errors.FirstOrDefault()?.Message);
        _mockJwtFactory.Verify(factory => factory.GenerateAccessToken(It.IsAny<AppUser>(), It.IsAny<IList<Claim>>()), Times.Never);
        _mockJwtFactory.Verify(factory => factory.GenerateRefreshToken(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleSignInCommand_WhenBadPassword_ShouldReturnSignInError()
    {
        // Arrange
        var user = null as AppUser;
        var command = _fixture.Create<SignInCommand>();

        _mockAuthService
            .GetUserIfValidPassword_Mock(command.UserName, command.Password, user);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsAssignableFrom<SignInError>(result.Errors.FirstOrDefault());
        Assert.Equal("Wrong username or password", result.Errors.FirstOrDefault()?.Message);
        _mockJwtFactory.Verify(factory => factory.GenerateAccessToken(It.IsAny<AppUser>(), It.IsAny<IList<Claim>>()), Times.Never);
        _mockJwtFactory.Verify(factory => factory.GenerateRefreshToken(It.IsAny<string>()), Times.Never);
    }
}
