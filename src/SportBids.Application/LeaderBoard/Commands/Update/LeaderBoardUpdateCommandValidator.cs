using FluentValidation;

namespace SportBids.Application.LeaderBoard.Commands.Update;

public class LeaderBoardUpdateCommandValidator : AbstractValidator<LeaderBoardUpdateCommand>
{
    public LeaderBoardUpdateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.LeaderBoardId)
            .NotEmpty();
        RuleFor(x => x.InitiatorId)
            .NotEmpty();
    }
}
