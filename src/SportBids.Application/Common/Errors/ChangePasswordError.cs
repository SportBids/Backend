namespace SportBids.Application.Common.Errors;

public class ChangePasswordError : BadRequestError
{
    public ChangePasswordError() : base("Failed to change password.")
    {
    }
}

