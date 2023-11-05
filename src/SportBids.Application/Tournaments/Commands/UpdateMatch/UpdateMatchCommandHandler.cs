using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application;

public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMatchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
    {
        var match = await _unitOfWork.Matches.FindAsync(request.TournamentId, request.MatchId, cancellationToken);

        if (match is null)
            return Result.Fail(new MatchNotFoundError(request.MatchId));

        if (match.Finished)
            return Result.Fail(new MatchReadOnlyError(match.Id));

        _mapper.Map(request, match);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
