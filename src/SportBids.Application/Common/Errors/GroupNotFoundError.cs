namespace SportBids.Application.Common.Errors;

public class GroupNotFoundError : BadRequestError
{
    public GroupNotFoundError(Guid id) : base($"Group '{id}' not found!")
    {
    }
}
