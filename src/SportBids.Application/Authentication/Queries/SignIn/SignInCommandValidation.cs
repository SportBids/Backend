using FluentValidation;
using SportBids.Application.Authentication.Commands.SignUp;

namespace SportBids.Application.Authentication.Queries.SignIn;

public class SignInCommandValidation : AbstractValidator<SignInCommand>
{
    public SignInCommandValidation()
    {
        RuleFor(x => x.UserName).MinimumLength(3);
        RuleFor(x => x.Password).NotEmpty();
    }
}
