using FluentValidation;

namespace SportBids.Application;

public class MatchPredictionsQueryValidator : AbstractValidator<MatchPredictionsQuery>
{
    public MatchPredictionsQueryValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.MatchIds).NotEmpty();
        RuleFor(x => x.MatchIds).ForEach(rule => rule.Must(y => y != Guid.Empty));
    }
}
