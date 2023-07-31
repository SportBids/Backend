using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Models;

namespace SportBids.Application.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public SendEmailConfirmationCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userRepository.FindByUsername(request.User.UserName);
        if (appUser is null) return;

        var body = await GetBody(appUser);
        var to = new[]
        {
            appUser.Email
        };
        await _emailService.SendAsync("Email confirmation", body, to, cancellationToken);
    }

    private async Task<string> GetBody(User user)
    {
        var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
        var link = "#";
        return $"Подтвердите регистрацию, перейдя по ссылке: <a href='{link}'>{user.Id} {token}</a>";
    }
}
