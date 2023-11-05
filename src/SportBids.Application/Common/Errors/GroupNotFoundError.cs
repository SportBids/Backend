using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class GroupNotFoundError : BadRequestError
{
    public GroupNotFoundError(Guid id) : base($"Group '{id}' not found!")
    {
    }
}
