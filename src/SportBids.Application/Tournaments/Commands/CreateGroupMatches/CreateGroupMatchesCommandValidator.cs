using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.CreateGroupMatches;

public class CreateGroupMatchesCommandValidator : AbstractValidator<CreateGroupMatchesCommand>
{
    public CreateGroupMatchesCommandValidator()
    {
        RuleFor(command => command.TournamentId)
            .NotEmpty();
    }
}
