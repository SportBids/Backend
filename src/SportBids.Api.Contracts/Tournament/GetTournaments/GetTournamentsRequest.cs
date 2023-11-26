namespace SportBids.Api.Contracts.Tournament.GetTournaments;

public class GetTournamentsRequest
{
    public bool IncludeNonPublic { get; set; } = false;
}
