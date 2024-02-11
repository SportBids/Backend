using System.Security.Claims;
using FluentResults;
using MediatR;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain;

namespace SportBids.Application.Accounts.Commands.SetUserRole;

public class SetUserRoleCommandHandler : IRequestHandler<SetUserRoleCommand, Result>
{
    private readonly IAuthService _authService;

    public SetUserRoleCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
    {
        Result result;
        if (request.Role == UserRoles.User)
            result = await _authService.RemoveUserClaimsAsync(request.UserId, new[] { ClaimTypes.Role });
        else
            result = await _authService.AddUserClaimsAsync(
                request.UserId, new[] { new Claim(ClaimTypes.Role, request.Role.ToString()) });
        return result;
    }
}
