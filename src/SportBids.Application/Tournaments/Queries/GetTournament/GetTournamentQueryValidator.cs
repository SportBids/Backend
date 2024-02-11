using FluentValidation;

namespace SportBids.Application.Tournaments.Queries.GetTournament;

public class GetTournamentQueryValidator : AbstractValidator<GetTournamentQuery>
{
    public GetTournamentQueryValidator()
    {
        RuleFor(x => x.TournamentId).NotEmpty();
    }
}
