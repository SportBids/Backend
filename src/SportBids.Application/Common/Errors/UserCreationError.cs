namespace SportBids.Application.Common.Errors;

public class UserCreationError : BadRequestError
{
    public UserCreationError() : base("Failed to register!")
    {
    }
}
