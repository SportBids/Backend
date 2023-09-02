using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IAuthService _authService;

    public ChangePasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(command.UserId);
        if (user is null)
        {
            return Result.Fail(new UserNotFoundError());
        }
        var response = await _authService.UpdatePasswordAsync(user.Id, command.CurrentPassword, command.NewPassword);
        return response;
    }
}
