using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SportBids.Domain.Entities;

public class RefreshToken
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public string Token { get; set; } = null!;
    public string CreatedByIp { get; set; } = null!;
    public string? RevokedByIp { get; set; }
    public string? ReasonRevoked { get; set; }
    public string? ReplacedByToken { get; set; }

    public DateTimeOffset Expires { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public bool IsExpired => DateTimeOffset.UtcNow > Expires;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}

