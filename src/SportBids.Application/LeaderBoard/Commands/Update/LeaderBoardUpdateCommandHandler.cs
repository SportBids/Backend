using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Commands.Update;

public class LeaderBoardUpdateCommandHandler : IRequestHandler<LeaderBoardUpdateCommand,Result<PrivateLeaderBoard>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LeaderBoardUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork  = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<PrivateLeaderBoard>> Handle(LeaderBoardUpdateCommand request, CancellationToken cancellationToken)
    {
        var leaderBoard = await _unitOfWork
                                .PrivateLeaderBoards
                                .GetByIdAsync(request.InitiatorId, request.LeaderBoardId, cancellationToken);
        if (leaderBoard is null)
            return Result.Fail(new PrivateLeaderBoardNotFoundError());
        _mapper.Map<LeaderBoardUpdateCommand, PrivateLeaderBoard>(request, leaderBoard);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok(leaderBoard);
    }
}
