using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class TournamentNotFoundError : BadRequestError
{
    public TournamentNotFoundError(Guid id) : base($"Tournament '{id}' not found!")
    {
    }
}
