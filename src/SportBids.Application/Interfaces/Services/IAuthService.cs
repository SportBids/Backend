using System.Security.Claims;
using FluentResults;
using SportBids.Application.Common.Models;
using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AppUser>> Create(AppUser user, string password);
    Task<AppUser?> FindByUsername(string username);
    Task<AppUser?> FindById(Guid userId);
    Task<int> GetUsersCountAsync();
    Task<AppUser?> GetUserIfValidPassword(string userName, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
    Task<bool> ConfirmEmailAsync(Guid userId, string emailConfirmationToken);
    Task<AppUser> UpdateAsync(AppUser user);
    Task<Result> UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<AppUser?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<IList<Claim>> GetClaimsAsync(AppUser user);
    Task<Result> AddUserClaimsAsync(Guid userId, IEnumerable<Claim> claims);
    Task<Result> AddUserClaimsAsync(AppUser user, IEnumerable<Claim> claims);
    Task<Result> RemoveUserClaimsAsync(Guid userId, IEnumerable<string> claimTypes);
    Task<Result> RemoveUserClaimsAsync(AppUser user, IEnumerable<string> claimTypes);
    Task<IEnumerable<AccountModel>> GetUsersAsync(CancellationToken cancellationToken);
}
