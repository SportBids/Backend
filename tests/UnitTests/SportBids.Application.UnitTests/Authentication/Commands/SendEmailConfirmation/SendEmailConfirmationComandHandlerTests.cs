using Moq;
using SportBids.Application.Authentication.Commands.SendEmailConfirmation;
using SportBids.Application.Interfaces.Services;
using SportBids.Application.UnitTests.Authentication.TestUtils;
using SportBids.Domain.Entities;

namespace SportBids.Application.UnitTests.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationComandHandlerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IEmailService> _mockEmailService;

    public SendEmailConfirmationComandHandlerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockEmailService = new Mock<IEmailService>();
    }

    [Fact]
    public async Task HandleSendEmailConfirmationCommand_ShouldCallEmailService()
    {
        // Arrange
        var command = new SendEmailConfirmationCommand
        {
            User = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "username",
                Email = "dont@email.me"
            }
        };
        string subjectParam = null!, bodyParam = null!;
        string[] toParam = null!;
        _mockAuthService.FindByUsername_Mock(command.User.UserName, command.User);
        _mockEmailService
            .Setup(service => service.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .Callback((string subject, string body, string[] to, CancellationToken token) =>
            {
                subjectParam = subject;
                bodyParam = body;
                toParam = to;
            });
        var sut = new SendEmailConfirmationCommandHandler(_mockAuthService.Object, _mockEmailService.Object);

        // Act
        await sut.Handle(command, default);

        // Assert
        _mockEmailService
            .Verify(service => service.SendAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                new[] { command.User.Email },
                It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal("Email confirmation", subjectParam);
        Assert.StartsWith("Подтвердите регистрацию, перейдя по ссылке: ", bodyParam);
        Assert.True(toParam.SequenceEqual(new[] { command.User.Email }));
    }
}
