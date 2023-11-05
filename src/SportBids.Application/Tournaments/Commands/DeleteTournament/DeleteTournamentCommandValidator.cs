using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.DeleteTournament;

public class DeleteTournamentCommandValidator : AbstractValidator<DeleteTournamentCommand>
{
    public DeleteTournamentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
