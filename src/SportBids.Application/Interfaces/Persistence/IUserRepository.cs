using FluentResults;
using SportBids.Application.Authentication.Common;
using SportBids.Domain.Models;

namespace SportBids.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<Result<User>> Create(User user, string password);
    Task<User?> FindByUsername(string username);
    Task<User?> GetUserIfValidPassword(string userName, string password);
}
