#nullable disable

using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Accounts.Commands.EditAccount;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Models;

namespace SportBids.Application.Accounts.Commands.UpdateAccount;

public class EditAccountCommandHandler : IRequestHandler<EditAccountCommand, Result<User>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public EditAccountCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<Result<User>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(request.UserId);
        if (user is null)
        {
            return Result.Fail<User>(new UserNotFoundError());
        }
        _mapper.Map<EditAccountCommand, User>(request, user);
        var response = await _authService.UpdateAsync(user);
        return Result.Ok(response);
    }
}
