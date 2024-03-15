using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.LeaderBoard.Commands.Join;

public class JoinPrivateLeaderBoardCommandHandler : IRequestHandler<JoinPrivateLeaderBoardCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public JoinPrivateLeaderBoardCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _unitOfWork       = unitOfWork;
        _authService = authService;
    }
    
    public async Task<Result> Handle(JoinPrivateLeaderBoardCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(request.UserId);
        if (user is null)
            return Result.Fail(new UserNotFoundError());
        var board = await _unitOfWork.PrivateLeaderBoards
                                     .GetByJoinCodeAsync(request.JoinCode, cancellationToken);
        if (board is null)
            return Result.Fail(new PrivateLeaderBoardNotFoundError());
        board.Members.Add(user);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
