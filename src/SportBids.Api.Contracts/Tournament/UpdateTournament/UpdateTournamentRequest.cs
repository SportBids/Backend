using System.ComponentModel.DataAnnotations;

namespace SportBids.Api.Contracts.Tournament.UpdateTournament;

public class UpdateTournamentRequest
{
    [Required]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime FinishAt { get; set; }
    public bool IsPublic { get; set; }
}
