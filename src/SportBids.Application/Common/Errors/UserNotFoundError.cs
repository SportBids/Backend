namespace SportBids.Application.Common.Errors;

public class UserNotFoundError : BadRequestError
{
    public UserNotFoundError() : base("User not found!")
    {
    }
}

