using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.LeaderBoard.Commands.Delete;

public class LeaderBoardDeleteCommandHandler : IRequestHandler<LeaderBoardDeleteCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaderBoardDeleteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LeaderBoardDeleteCommand request, CancellationToken cancellationToken)
    {
        var board = await _unitOfWork
                          .PrivateLeaderBoards
                          .GetByIdAsync(
                              request.InitiatorId, request.BoardId, cancellationToken);
        if (board is null)
            return Result.Fail(new PrivateLeaderBoardNotFoundError());
        _unitOfWork.PrivateLeaderBoards.Delete(board);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
