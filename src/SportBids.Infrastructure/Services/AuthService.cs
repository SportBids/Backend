using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportBids.Application.Common.Errors;
using SportBids.Application.Common.Models;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;

    public AuthService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> AddUserClaimsAsync(Guid userId, IEnumerable<Claim> claims)
    {
        var user = await FindById(userId);
        if (user is null)
            return Result.Fail(new UserNotFoundError());

        return await AddUserClaimsAsync(user, claims);
    }
    
    public async Task<Result> AddUserClaimsAsync(AppUser user, IEnumerable<Claim> claims)
    {
        var identityResult = await _userManager.AddClaimsAsync(user, claims);
        return identityResult.Succeeded
            ? Result.Ok()
            : Result.Fail(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<Result> RemoveUserClaimsAsync(Guid userId, IEnumerable<string> claimTypes)
    {
        var user = await FindById(userId);
        if (user is null)
            return Result.Fail(new UserNotFoundError());

        return await RemoveUserClaimsAsync(user, claimTypes);
    }
    
    public async Task<Result> RemoveUserClaimsAsync(AppUser user, IEnumerable<string> claimTypes)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var claimsToRemove = userClaims.Where(c => claimTypes.Contains(c.Type));
        var identityResult = await _userManager.RemoveClaimsAsync(user, claimsToRemove);

        return identityResult.Succeeded
            ? Result.Ok()
            : Result.Fail(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<IEnumerable<AccountModel>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userManager
            .Users
            .ToListAsync(cancellationToken);
        var admins = await _userManager
            .GetUsersForClaimAsync(new Claim(ClaimTypes.Role, UserRoles.Administrator.ToString()));

        var moderators = await _userManager
            .GetUsersForClaimAsync(new Claim(ClaimTypes.Role, UserRoles.Moderator.ToString()));

        var accounts = users
            .Select(
                user => new AccountModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = admins.Contains(user)
                        ? UserRoles.Administrator.ToString()
                        : moderators.Contains(user)
                            ? UserRoles.Moderator.ToString()
                            : string.Empty
                })
            .ToList();
        return accounts;
    }


    // public async Task<Result> SetUserRoleAsync(Guid userId, UserClaims role, CancellationToken cancellationToken)
    // {
    //     var user = await _userManager.FindByIdAsync(userId.ToString());
    //
    //     if (user is null)
    //         return Result.Fail(new UserNotFoundError());
    //
    //     var currentRoles = await _userManager.GetClaimsAsync(user);
    //     if (currentRoles.Any(x => x.Type == ClaimTypes.Role && x.Value == role.ToString()))
    //         return Result.Ok();
    //
    //     if (currentRoles.Count > 0)
    //     {
    //         var r = await _userManager.RemoveFromRolesAsync(user, currentRoles);
    //         if (!r.Succeeded)
    //             return Result.Fail(r.Errors.Select(e => e.Description).ToArray());
    //     }
    //
    //     var result = await _userManager.AddToRoleAsync(user, role.ToString());
    //     return result.Succeeded
    //         ? Result.Ok()
    //         : Result.Fail(result.Errors.Select(e => e.Description).ToArray());
    // }

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

    public async Task<IList<Claim>> GetClaimsAsync(AppUser user)
    {
        return await _userManager.GetClaimsAsync(user);
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

    public async Task<int> GetUsersCountAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<AppUser> UpdateAsync(AppUser user)
    {
        _ = await _userManager.UpdateAsync(user);
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
