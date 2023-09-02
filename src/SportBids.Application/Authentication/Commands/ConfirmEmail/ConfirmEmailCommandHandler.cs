using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IAuthService _authService;

    public ConfirmEmailCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }


    async Task<Result> IRequestHandler<ConfirmEmailCommand, Result>.Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var isConfirmed = await _authService.ConfirmEmailAsync(request.UserId, request.Token);
        if (!isConfirmed)
            return Result.Fail(new EmailConfirmationError());

        return Result.Ok();
    }
}
