using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result>
{
    private readonly IAuthService _authService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RevokeTokenCommandHandler(IAuthService authService, IDateTimeProvider dateTimeProvider)
    {
        _authService = authService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null)
            return Result.Fail(new BadRefreshToken());

        var refreshToken = user.RefreshTokens.Single(rt => rt.Token == request.RefreshToken);
        if (!refreshToken.IsActive)
        {
            return Result.Fail(new BadRefreshToken());
        }
        refreshToken.RevokedAt = _dateTimeProvider.UtcNow;
        refreshToken.ReasonRevoked = "Revoked without replacement";
        refreshToken.RevokedByIp = request.IPAddress;
        refreshToken.ReplacedByToken = null;

        await _authService.UpdateAsync(user);
        return Result.Ok();
    }
}
