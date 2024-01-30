namespace SportBids.Api.Contracts.Tournament.GetTournaments;

public class TournamentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsPublic { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset FinishAt { get; set; }
    public bool IsFinished { get; set; }
}
