using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class MatchReadOnlyError : BadRequestError
{
    public MatchReadOnlyError(Guid id) : base($"Match '{id}' cannot be modified!")
    {
    }
}
