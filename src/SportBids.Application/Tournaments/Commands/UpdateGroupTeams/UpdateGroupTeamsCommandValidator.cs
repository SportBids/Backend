using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.UpdateGroupTeams;

public class UpdateGroupTeamsCommandValidator : AbstractValidator<UpdateGroupTeamsCommand>
{
    public UpdateGroupTeamsCommandValidator()
    {
        RuleFor(command => command.TournamentId)
            .NotEmpty();
        RuleFor(command => command.GroupId)
            .NotEmpty();
        RuleFor(command => command.TeamIds)
            .NotEmpty()
            .ForEach(teamId => teamId.NotEmpty());
    }
}
