using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    async Task<Result> IRequestHandler<ConfirmEmailCommand, Result>.Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var isConfirmed = await _userRepository.ConfirmEmailAsync(request.UserId, request.Token);
        if (!isConfirmed)
            return Result.Fail(new EmailConfirmationError());

        return Result.Ok();
    }
}
