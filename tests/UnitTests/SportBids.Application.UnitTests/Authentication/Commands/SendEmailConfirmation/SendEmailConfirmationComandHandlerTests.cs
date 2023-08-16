using Moq;
using SportBids.Application.Authentication.Commands.SendEmailConfirmation;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Models;
using SportBids.Application.UnitTests.Authentication.TestUtils;

namespace SportBids.Application.UnitTests.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationComandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IEmailService> _mockEmailService;

    public SendEmailConfirmationComandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockEmailService = new Mock<IEmailService>();
    }

    [Fact]
    public async Task HandleSendEmailConfirmationCommand_ShouldCallEmailService()
    {
        // Arrange
        var command = new SendEmailConfirmationCommand
        {
            User = new User
            {
                Id = Guid.NewGuid(),
                UserName = "username",
                Email = "dont@email.me"
            }
        };
        string subjectParam = null!, bodyParam = null!;
        string[] toParam = null!;
        _mockUserRepository.FindByUsername_Mock(command.User.UserName, command.User);
        _mockEmailService
            .Setup(service => service.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .Callback((string subject, string body, string[] to, CancellationToken token) =>
            {
                subjectParam = subject;
                bodyParam = body;
                toParam = to;
            });
        var sut = new SendEmailConfirmationCommandHandler(_mockUserRepository.Object, _mockEmailService.Object);

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
