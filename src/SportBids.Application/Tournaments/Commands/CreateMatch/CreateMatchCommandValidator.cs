using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.CreateMatch;

public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.TournamentId).NotEmpty();
        RuleFor(x => x.Team1Id).NotEmpty();
        RuleFor(x => x.Team2Id).NotEmpty();
    }
}
