using FluentValidation;

namespace SportBids.Application;

public class GetTournamentQueryValidator : AbstractValidator<GetTournamentQuery>
{
    public GetTournamentQueryValidator()
    {
        RuleFor(x => x.TournamentId).NotEmpty();
    }
}
