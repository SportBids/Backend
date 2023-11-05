using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class MatchNotFoundError : BadRequestError
{
    public MatchNotFoundError(Guid id) : base($"Match '{id}' not found!")
    {
    }
}
