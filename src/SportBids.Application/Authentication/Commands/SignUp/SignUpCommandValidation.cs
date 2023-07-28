using FluentValidation;

namespace SportBids.Application.Authentication.Commands.SignUp;

public class SignUpCommandValidation : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidation()
    {
        RuleFor(x => x.FirstName).MaximumLength(50);
        RuleFor(x => x.LastName).MaximumLength(50);
        RuleFor(x => x.UserName).MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
