using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class GroupTeamsMissingError : BadRequestError
{
    public GroupTeamsMissingError(string groupName) : base($"Not enough teams in group {groupName}")
    { }
}
