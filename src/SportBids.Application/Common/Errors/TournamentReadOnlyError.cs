namespace SportBids.Application.Common.Errors;

public class TournamentReadOnlyError : BadRequestError
{
    public TournamentReadOnlyError(Guid id) : base($"Tournament {id} is public. Cannot be modified.")
    {
    }
}
