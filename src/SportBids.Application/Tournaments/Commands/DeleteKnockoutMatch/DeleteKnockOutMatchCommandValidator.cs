using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.DeleteKnockoutMatch;

public class DeleteKnockOutMatchCommandValidator : AbstractValidator<DeleteKnockOutMatchCommand>
{
    public DeleteKnockOutMatchCommandValidator()
    {
        RuleFor(x => x.TournamentId).NotEmpty();
        RuleFor(x => x.MatchId).NotEmpty();
    }
}
