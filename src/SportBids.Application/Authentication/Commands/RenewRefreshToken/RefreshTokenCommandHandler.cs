using System.Net;
using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Commands.RenewRefreshToken;

public class RenewRefreshTokenCommandHandler : IRequestHandler<RenewRefreshTokenCommand, Result<AuthResult>>
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

    public async Task<Result<AuthResult>> Handle(RenewRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindUserByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null)
        {
            return Result.Fail<AuthResult>(new UserNotFoundError());
        }

        //var refreshToken = _authService.GetRefreshToken(user, request.RefreshToken);
        var refreshToken = user.RefreshTokens.Single(rt => rt.Token == request.RefreshToken);

        if (refreshToken.IsRevoked)
        {
            RevokeDescendantRefreshTokens(refreshToken, user, request.IPAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
        }

        if (!refreshToken.IsActive)
        {
            return Result.Fail<AuthResult>(new BadRefreshToken());
        }

        RefreshToken newRefreshToken = RotateRefreshToken(refreshToken, request.IPAddress);

        user.RefreshTokens.Add(newRefreshToken);

        RemoveOldRefreshTokens(user);

        await _authService.UpdateAsync(user);

        var authResult = _mapper.Map<AuthResult>(user);
        authResult.AccessToken = _jwtFactory.GenerateAccessToken(user.Id);
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

            if (childToken.IsActive)
                RevokeRefreshToken(childToken, ipAddress, reason);
            else
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
