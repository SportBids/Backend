using System.Collections;
using SportBids.Api.Contracts.Account.ListAccounts;

namespace SportBids.Api.Contracts.LeaderBoards.List;

public class LeaderBoardDto
{
    public IEnumerable<LeaderBoardItemDto> Items { get; set; } = null!;
}

public class LeaderBoardItemDto
{
    public AccountDto User { get; init; } = null!;
    public int Points { get; set; }
}

public class PrivateLeaderBoardShortDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}
