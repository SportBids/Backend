#nullable disable

using FluentValidation;

namespace SportBids.Application.Tournaments.Commands.CreateTournament;

public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
{
    public CreateTournamentCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.NumberOfGroups)
            .GreaterThanOrEqualTo(1);

        RuleFor(command => command.NumberOfTeams)
            .GreaterThanOrEqualTo(2);

        RuleFor(command => command.NumberOfGroups)
            .Must((command, numberOfGroups) => command.NumberOfTeams % numberOfGroups == 0)
            .When(command => command.NumberOfGroups > 0)
            .WithMessage("Количество команд должно делиться на количество групп.");

        RuleFor(command => command.StartAt)
            .NotEqual(default(DateTime));

        RuleFor(command => command.FinishAt)
            .GreaterThan(DateTime.Now);

        RuleFor(command => command.StartAt)
            .Must((command, startAt) => startAt < command.FinishAt)
            .WithMessage("Дата начала турнира должна быть раньше даты окончания турнира.");
    }
}
