namespace SportBids.Application.Common.Errors;

public class BadRefreshToken : BadRequestError
{
    public BadRefreshToken() : base("Invalid token")
    {
    }
}

