using FluentValidation;

namespace SportBids.Application.LeaderBoard.Commands.Create;

public class LeaderBoardCreateCommandValidator : AbstractValidator<LeaderBoardCreateCommand>
{
    public LeaderBoardCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.OwnerId)
            .NotEmpty();
    }
}
