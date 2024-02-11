namespace SportBids.Application.Common.Errors;

public class EmailConfirmationError : BadRequestError
{
    public EmailConfirmationError() : base("Failed to verify email confirmation token")
    {
    }
}
