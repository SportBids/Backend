namespace SportBids.Application.Common.Errors;

public class TournamentNotFoundError : BadRequestError
{
    public TournamentNotFoundError(Guid id) : base($"Tournament '{id}' not found!")
    {
    }
}
