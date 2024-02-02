using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Commands.RenewJwt;

public class RenewRefreshTokenCommandHandler : IRequestHandler<RenewJwtCommand, Result<AuthResult>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IJwtFactory _jwtFactory;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RenewRefreshTokenCommandHandler(IAuthService authService, IMapper mapper, IJwtFactory jwtFactory, IDateTimeProvider dateTimeProvider)
    {
        _authService = authService;
        _mapper = mapper;
        _jwtFactory = jwtFactory;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<AuthResult>> Handle(RenewJwtCommand command, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByRefreshTokenAsync(command.RefreshToken, cancellationToken);
        if (user is null)
        {
            return Result.Fail<AuthResult>(new UserNotFoundError());
        }

        //var refreshToken = _authService.GetRefreshToken(user, request.RefreshToken);
        var refreshToken = user.RefreshTokens.Single(rt => rt.Token == command.RefreshToken);

        if (refreshToken.IsRevoked)
        {
            RevokeDescendantRefreshTokens(refreshToken, user, command.IPAddress, $"Attempted reuse of revoked ancestor token: {command.RefreshToken}");
        }

        if (!refreshToken.IsActive)
        {
            return Result.Fail<AuthResult>(new BadRefreshToken());
        }

        var newRefreshToken = RotateRefreshToken(refreshToken, command.IPAddress);

        user.RefreshTokens.Add(newRefreshToken);

        RemoveOldRefreshTokens(user);

        await _authService.UpdateAsync(user);

        var authResult = _mapper.Map<AuthResult>(user);
        var userClaims = await _authService.GetClaimsAsync(user);
        authResult.AccessToken = _jwtFactory.GenerateAccessToken(user, userClaims);
        authResult.RefreshToken = newRefreshToken.Token;

        return Result.Ok(authResult);
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, AppUser user, string ipAddress, string reason)
    {
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

            if (childToken is null)
                return;

            if (!childToken.IsRevoked)
                RevokeRefreshToken(childToken, ipAddress, reason);
            RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtFactory.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
    {
        token.ReplacedByToken = replacedByToken;
        token.RevokedByIp = ipAddress;
        token.RevokedAt = _dateTimeProvider.UtcNow;
        token.ReasonRevoked = reason;
    }

    private void RemoveOldRefreshTokens(AppUser user)
    {
        user.RefreshTokens
            .RemoveAll(rt => rt.IsExpired);
    }
}
