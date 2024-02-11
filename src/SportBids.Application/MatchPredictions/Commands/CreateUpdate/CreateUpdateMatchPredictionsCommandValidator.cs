using FluentValidation;

namespace SportBids.Application.MatchPredictions.Commands.CreateUpdate;

public class CreateUpdateMatchPredictionsCommandValidator : AbstractValidator<CreateUpdateMatchPredictionsCommand>
{
    public CreateUpdateMatchPredictionsCommandValidator()
    {
        RuleFor(x => x.Predictions)
            .NotEmpty()
            .ForEach(
                rule =>
                {
                    rule.Must(p => p.MatchId != Guid.Empty);
                    rule.Must(p => p.Score.Team1 >= 0);
                    rule.Must(p => p.Score.Team2 >= 0);
                    rule.Must(p => p.Score.Team1 <= 15);
                    rule.Must(p => p.Score.Team2 <= 15);
                });
        RuleFor(x => x.CreatedById).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
