using FluentResults;
using SportBids.Domain.Models;

namespace SportBids.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<Result<User>> Create(User user, string password);
    Task<User?> FindByUsername(string username);
    Task<User?> FindById(Guid userId);
    Task<User?> GetUserIfValidPassword(string userName, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<bool> ConfirmEmailAsync(Guid userId, string emailConfirmationToken);
    Task<User> UpdateAsync(User user);
    Task<Result> UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);
}
