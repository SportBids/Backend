using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.UpdateTournament;


public class UpdateTournamentCommandValidator : AbstractValidator<UpdateTournamentCommand>
{
    public UpdateTournamentCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.StartAt)
            .NotEqual(default(DateTime));

        RuleFor(command => command.FinishAt)
            .GreaterThan(DateTime.Now);

        RuleFor(command => command.StartAt)
            .Must((command, startAt) => startAt < command.FinishAt)
            .WithMessage("Дата начала турнира должна быть раньше даты окончания турнира.");
    }
}
