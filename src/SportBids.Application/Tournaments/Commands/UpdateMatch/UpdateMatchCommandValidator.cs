using FluentValidation;

namespace SportBids.Application;

public class UpdateMatchCommandValidator : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.TournamentId)
            .NotEmpty();
        RuleFor(x => x.MatchId)
            .NotEmpty();
        RuleFor(x => x.Team1Id)
            .NotEmpty();
        RuleFor(x => x.Team2Id)
            .NotEmpty();
        RuleFor(x => x.StartAt)
            .NotEmpty();
        RuleFor(x => x.Score.Team1)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.Score.Team2)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.Team1Id)
            .NotEqual(x => x.Team2Id);
    }
}
