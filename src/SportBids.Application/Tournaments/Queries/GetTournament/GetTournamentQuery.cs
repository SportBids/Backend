using FluentResults;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Application.Tournaments.Queries.GetTournament;

public class GetTournamentQuery : IRequest<Result<Tournament>>
{
    public Guid TournamentId { get; set; }
}

public class GetTournamentQueryHandler : IRequestHandler<GetTournamentQuery, Result<Tournament>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTournamentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Tournament>> Handle(GetTournamentQuery request, CancellationToken cancellationToken)
    {
        var tournament = await _unitOfWork.Tournaments.GetTournamentFullInfo(request.TournamentId, cancellationToken);
        if (tournament is null)
            return Result.Fail(new TournamentNotFoundError(request.TournamentId));
        return Result.Ok(tournament);
    }
}
