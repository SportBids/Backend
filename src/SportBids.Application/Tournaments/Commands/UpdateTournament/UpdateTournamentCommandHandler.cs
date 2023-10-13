using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain;

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

        if (tournamentEntity.IsPublic)
        {
            return Result.Fail("Cannot update public tournament");
        }

        _mapper.Map(command, tournamentEntity);
        _unitOfWork.Tournaments.Update(tournamentEntity);
        await _unitOfWork.SaveAsync();

        return Result.Ok(tournamentEntity.Id);
    }
}
