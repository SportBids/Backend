using FluentValidation;

namespace SportBids.Application.LeaderBoard.Commands.Delete;

public class LeaderBoardDeleteCommandValidator : AbstractValidator<LeaderBoardDeleteCommand>
{
    public LeaderBoardDeleteCommandValidator()
    {
        RuleFor(x => x.BoardId)
            .NotEmpty();
        RuleFor(x => x.InitiatorId)
            .NotEmpty();
    }
}
