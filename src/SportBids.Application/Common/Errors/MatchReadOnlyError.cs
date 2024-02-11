namespace SportBids.Application.Common.Errors;

public class MatchReadOnlyError : BadRequestError
{
    public MatchReadOnlyError(Guid id) : base($"Match '{id}' cannot be modified!")
    {
    }
}
