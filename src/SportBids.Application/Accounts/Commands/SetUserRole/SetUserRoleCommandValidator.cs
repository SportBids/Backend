// #nullable disable
// using SportBids.Domain;
// using FluentValidation;
//
// namespace SportBids.Application.Accounts.Commands.SetUserRole;
//
// public class SetUserRoleCommandValidator : AbstractValidator<SetUserRoleCommand>
// {
//     public SetUserRoleCommandValidator()
//     {
//         RuleFor(command => command.Role)
//             .Must(x => string.IsNullOrWhiteSpace(x) || x == UserRoles.Administrator || x == UserRoles.Moderator);
//     }
// }
