namespace SportBids.Application.Common.Errors;

public class TeamNotFoundError : BadRequestError
{
    public TeamNotFoundError(Guid id) : base($"Team '{id}' not found!")
    {
    }
}
