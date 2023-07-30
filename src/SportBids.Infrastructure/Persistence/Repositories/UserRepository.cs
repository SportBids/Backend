using FluentResults;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using SportBids.Application.Authentication.Common;
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

    public async Task<User?> FindByUsername(string username)
    {
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser is null)
            return null;
        return _mapper.Map<User>(appUser);
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
}
