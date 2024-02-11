namespace SportBids.Application.Common.Errors;

public class MatchNotFoundError : BadRequestError
{
    public MatchNotFoundError(Guid id) : base($"Match '{id}' not found!")
    {
    }
}
