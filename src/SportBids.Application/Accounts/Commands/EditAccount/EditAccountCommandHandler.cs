#nullable disable

using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Accounts.Commands.EditAccount;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Application.Accounts.Commands.UpdateAccount;

public class EditAccountCommandHandler : IRequestHandler<EditAccountCommand, Result<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public EditAccountCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<User>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindById(request.UserId);
        if (user is null)
        {
            return Result.Fail<User>(new UserNotFoundError());
        }
        _mapper.Map<EditAccountCommand, User>(request, user);
        var response = await _userRepository.UpdateAsync(user);
        return Result.Ok(response);
    }
}
