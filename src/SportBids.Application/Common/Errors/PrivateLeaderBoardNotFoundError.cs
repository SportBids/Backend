using FluentResults;

namespace SportBids.Application.Common.Errors;

public class PrivateLeaderBoardNotFoundError : BadRequestError
{
    public PrivateLeaderBoardNotFoundError() : base("LeaderBoard not found!")
    {
    }
}
