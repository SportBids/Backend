using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AuthService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, string emailConfirmationToken)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null)
            return false;

        var identityResult = await _userManager.ConfirmEmailAsync(appUser, emailConfirmationToken);
        return identityResult.Succeeded;
    }

    public async Task<Result<AppUser>> Create(AppUser user, string password)
    {
        var identityResult = await _userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            var error = new UserCreationError();
            foreach (var identityError in identityResult.Errors)
            {
                error.WithMetadata(identityError.Code, identityError.Description);
            }

            return Result.Fail<AppUser>(error);
        }

        return Result.Ok(user);
    }

    public async Task<AppUser?> FindById(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<AppUser?> FindByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<AppUser?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _userManager
            .Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(user => user.RefreshTokens.Any(rt => rt.Token == refreshToken), cancellationToken);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    //public RefreshToken GetRefreshToken(AppUser user, string refreshToken)
    //{
    //    return user.RefreshTokens.Single(rt => rt.Token == refreshToken);
    //}

    public async Task<AppUser?> GetUserIfValidPassword(string userName, string password)
    {
        var appUser = await _userManager.FindByNameAsync(userName);
        if (appUser is null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(appUser, password);
        if (!isPasswordValid)
            return null;

        return appUser;
    }

    public async Task<AppUser> UpdateAsync(AppUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        return user;
    }

    public async Task<Result> UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser is null)
            throw new InvalidOperationException();
        var result = await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

        if (result.Succeeded)
        {
            return Result.Ok();
        }

        var error = new ChangePasswordError();
        foreach (var resultError in result.Errors)
        {
            error.WithMetadata(resultError.Code, resultError.Description);
        }

        return Result.Fail(error);
    }
}
