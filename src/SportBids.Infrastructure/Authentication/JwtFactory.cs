﻿using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Authentication;

public class JwtFactory : IJwtFactory
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtFactory(IOptions<JwtSettings> jwtSettingsOptions, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public string GenerateAccessToken(AppUser user, IEnumerable<Claim> userClaims)
    {
        var claims = GetClaims(user.Id).Concat(userClaims).ToArray();
        var signingCredentials = GetSigningCredentials();
        var securityToken = GetSecurityToken(claims, signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public RefreshToken GenerateRefreshToken(string IPAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            CreatedByIp = IPAddress,
            CreatedAt = _dateTimeProvider.UtcNow,
            Expires = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryMinutes),
        };

        return refreshToken;
    }

    private SigningCredentials GetSigningCredentials()
    {
        // var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        // var securityKey = new SymmetricSecurityKey(key);
        var securityKey = SecurityKeyProvider.GetSecurityKey(_jwtSettings);
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    private Claim[] GetClaims(Guid userId)
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        return claims;
    }

    private SecurityToken GetSecurityToken(Claim[] claims, SigningCredentials signingCredentials)
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: _dateTimeProvider.UtcNow.DateTime,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes).LocalDateTime,
            signingCredentials: signingCredentials);
    }
}
