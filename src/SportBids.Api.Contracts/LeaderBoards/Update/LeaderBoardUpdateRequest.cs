#nullable disable

namespace SportBids.Api.Contracts.LeaderBoards.Update;

public class LeaderBoardUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
