using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTeamCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _unitOfWork.Teams.GetTeamAsync(request.TournamentId, request.TeamId, cancellationToken);
        if (team is null)
            return Result.Fail(new TeamNotFoundError(request.TeamId));
        _mapper.Map(request, team);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
