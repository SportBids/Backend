using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Application.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public ChangePasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindById(command.UserId);
        if (user is null)
        {
            return Result.Fail(new UserNotFoundError());
        }
        var response = await _userRepository.UpdatePasswordAsync(user.Id, command.CurrentPassword, command.NewPassword);
        return response;
    }
}
