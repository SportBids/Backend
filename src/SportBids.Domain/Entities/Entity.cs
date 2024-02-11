#nullable disable

namespace SportBids.Domain.Entities;

public abstract class Entity<TId>
{
    public TId Id { get; set; }
}
