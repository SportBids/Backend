using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(UserManager<AppUser> userManager, IMapper mapper)
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

    public async Task<Result<User>> Create(User user, string password)
    {
        var newUser = _mapper.Map<AppUser>(user);
        var identityResult = await _userManager.CreateAsync(newUser, password);

        if (!identityResult.Succeeded)
        {
            var error = new UserCreationError();
            foreach (var identityError in identityResult.Errors)
            {
                error.WithMetadata(identityError.Code, identityError.Description);
            }

            return Result.Fail<User>(error);
        }

        user = _mapper.Map<User>(newUser);
        return Result.Ok(user);
    }

    public async Task<User?> FindById(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser is null)
        {
            return null;
        }
        return _mapper.Map<User>(appUser);
    }

    public async Task<User?> FindByUsername(string username)
    {
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser is null)
            return null;
        return _mapper.Map<User>(appUser);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var appUser = await _userManager.FindByNameAsync(user.UserName);
        if (appUser is null)
            return string.Empty;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        return token;
    }

    public async Task<User?> GetUserIfValidPassword(string userName, string password)
    {
        var appUser = await _userManager.FindByNameAsync(userName);
        if (appUser is null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(appUser, password);
        if (!isPasswordValid)
            return null;

        return _mapper.Map<User>(appUser);
    }

    public async Task<User> UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null)
            throw new InvalidOperationException();
        _mapper.Map<User, AppUser>(user, appUser);
        var result = await _userManager.UpdateAsync(appUser);
        return _mapper.Map<User>(appUser);
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
