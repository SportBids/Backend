using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.LeaderBoard.Commands.Create;

public class LeaderBoardCreateCommandHandler : IRequestHandler<LeaderBoardCreateCommand, Result<PrivateLeaderBoard>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public LeaderBoardCreateCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _unitOfWork  = unitOfWork;
        _authService = authService;
    }
    
    public async Task<Result<PrivateLeaderBoard>> Handle(LeaderBoardCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.FindById(request.OwnerId);
        if (user is null) return Result.Fail(new UserNotFoundError());

        var leaderBoard = new PrivateLeaderBoard()
        {
            Name     = request.Name,
            Owner    = user,
            JoinCode = Guid.NewGuid().ToString(),
            Members  = new List<AppUser> { user }
        };
        
        _unitOfWork.PrivateLeaderBoards.Add(leaderBoard);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok(leaderBoard);
    }
}
