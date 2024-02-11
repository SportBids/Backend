using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.Tournaments.Commands.UpdateTournament;

public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTournamentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(UpdateTournamentCommand command, CancellationToken cancellationToken)
    {
        var tournamentEntity = await _unitOfWork.Tournaments.GetByIdAsync(command.Id, cancellationToken);
        if (tournamentEntity is null)
            return Result.Fail(new TournamentNotFoundError(command.Id));

        // if (tournamentEntity.IsPublic)
        // {
        //     return Result.Fail(new TournamentReadOnlyError(tournamentEntity.Id));
        // }

        _mapper.Map(command, tournamentEntity);
        _unitOfWork.Tournaments.Update(tournamentEntity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok(tournamentEntity.Id);
    }
}
