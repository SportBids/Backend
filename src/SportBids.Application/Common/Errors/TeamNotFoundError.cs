using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class TeamNotFoundError : BadRequestError
{
    public TeamNotFoundError(Guid id) : base($"Team '{id}' not found!")
    {
    }
}
