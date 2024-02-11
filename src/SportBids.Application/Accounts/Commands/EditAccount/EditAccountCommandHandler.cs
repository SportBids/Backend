#nullable disable

using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.Accounts.Commands.EditAccount;

public class EditAccountCommandHandler : IRequestHandler<EditAccountCommand, Result<AppUser>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public EditAccountCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<Result<AppUser>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(request.UserId);
        if (user is null)
        {
            return Result.Fail<AppUser>(new UserNotFoundError());
        }
        _mapper.Map<EditAccountCommand, AppUser>(request, user);
        var response = await _authService.UpdateAsync(user);
        return Result.Ok(response);
    }
}
