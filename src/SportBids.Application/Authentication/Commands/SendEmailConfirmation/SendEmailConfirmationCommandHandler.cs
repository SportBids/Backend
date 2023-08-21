using MediatR;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    public SendEmailConfirmationCommandHandler(IAuthService authService, IEmailService emailService)
    {
        _emailService = emailService;
        _authService = authService;
    }

    public async Task Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _authService.FindByUsername(request.User.UserName!);
        if (appUser is null) return;

        var body = await GetBody(appUser);
        var to = new string[]
        {
            appUser.Email!
        };
        await _emailService.SendAsync("Email confirmation", body, to, cancellationToken);
    }

    private async Task<string> GetBody(AppUser user)
    {
        var token = await _authService.GenerateEmailConfirmationTokenAsync(user);
        var link = "#";
        return $"Подтвердите регистрацию, перейдя по ссылке: <a href='{link}'>{user.Id} {token}</a>";
    }
}
