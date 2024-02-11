using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Entities;

namespace SportBids.Application.Tournaments.Queries.GetTournaments;

public class GetTournamentsQueryHandler : IRequestHandler<GetTournamentsQuery, IEnumerable<Tournament>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTournamentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Tournament>> Handle(GetTournamentsQuery request, CancellationToken cancellationToken)
    {
        // var tournaments = await _unitOfWork.Tournaments.GetAllAsync(cancellationToken);
        var tournaments = request.IncludeNonPublic
            ? await _unitOfWork.Tournaments.GetAsync(cancellationToken)
            : await _unitOfWork.Tournaments.GetPublicOnlyAsync(cancellationToken);
        return tournaments;
    }
}
