using SportBids.Domain.Entities;

namespace SportBids.Application.Common.Models;

public class LeaderBoardModel
{
    public IEnumerable<LeaderBoardItemModel> Items { get; init; } = null!;
}

public class LeaderBoardItemModel
{
    public AppUser User { get; init; } = null!;
    public int Points { get; set; }
}
