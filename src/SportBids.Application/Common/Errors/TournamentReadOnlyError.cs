using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class TournamentReadOnlyError : BadRequestError
{
    public TournamentReadOnlyError(Guid id) : base($"Tournament {id} is public. Cannot be modified.")
    {
    }
}
