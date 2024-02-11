namespace SportBids.Application.Common.Errors;

public class GroupTeamsMissingError : BadRequestError
{
    public GroupTeamsMissingError(string groupName) : base($"Not enough teams in group {groupName}")
    {
    }
}
