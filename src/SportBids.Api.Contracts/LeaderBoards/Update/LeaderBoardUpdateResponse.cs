#nullable disable

namespace SportBids.Api.Contracts.LeaderBoards.Update;

public class LeaderBoardUpdateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string JoinCode { get; set; }
}
