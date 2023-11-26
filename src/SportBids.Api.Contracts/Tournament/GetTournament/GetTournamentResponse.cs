#nullable disable

namespace SportBids.Api.Contracts.Tournament.GetTournament;

public class GetTournamentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsPublic { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset FinishAt { get; set; }
    public ICollection<TeamDto> Teams { get; set; } = null!;
    public ICollection<GroupDto> Groups { get; set; } = null!;
    public ICollection<MatchDto>? KnockOutMatches { get; set; }
    public bool IsFinished { get; set; }
}

public class MatchDto
{
    public Guid Id { get; set; }
    public TeamDto Team1 { get; set; } = null!;
    public TeamDto Team2 { get; set; } = null!;
    public DateTimeOffset StartAt { get; set; }
    public bool Finished { get; set; }
    public ScoreDto Score { get; set; }
}

public struct ScoreDto
{
    public int Team1 { get; set; }
    public int Team2 { get; set; }
}

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<TeamDto> Teams { get; set; }
    public ICollection<MatchDto> Matches { get; set; }

}

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsKnockedOut { get; set; }
}
