using FluentResults;
using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AppUser>> Create(AppUser user, string password);
    Task<AppUser?> FindByUsername(string username);
    Task<AppUser?> FindById(Guid userId);
    Task<AppUser?> GetUserIfValidPassword(string userName, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
    Task<bool> ConfirmEmailAsync(Guid userId, string emailConfirmationToken);
    Task<AppUser> UpdateAsync(AppUser user);
    Task<Result> UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<AppUser?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
