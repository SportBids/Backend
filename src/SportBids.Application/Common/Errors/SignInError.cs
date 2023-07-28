namespace SportBids.Application.Common.Errors;

public class SignInError : BadRequestError
{
    public SignInError() : base("Wrong username or password")
    {
    }
}
