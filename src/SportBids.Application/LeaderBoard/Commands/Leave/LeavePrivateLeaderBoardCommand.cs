using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.LeaderBoard.Commands.Leave;

public class LeavePrivateLeaderBoardCommand : IRequest<Result>
{
    public Guid UserId { get; init; }
    public Guid BoardId { get; init; }
}

public class LeavePrivateLeaderBoardCommandHandler : IRequestHandler<LeavePrivateLeaderBoardCommand, Result>
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public LeavePrivateLeaderBoardCommandHandler(IAuthService authService, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _unitOfWork  = unitOfWork;
    }

    public async Task<Result> Handle(LeavePrivateLeaderBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(request.UserId);
        if (user is null)
            return Result.Fail(new UserNotFoundError());
        var board = await _unitOfWork.PrivateLeaderBoards
                                     .GetByIdWithMembersAsync(request.BoardId, cancellationToken);
        if (board is null)
            return Result.Fail(new PrivateLeaderBoardNotFoundError());
        board.Members.Remove(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
