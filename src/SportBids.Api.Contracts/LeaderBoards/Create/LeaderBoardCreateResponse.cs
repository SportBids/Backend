#nullable disable

namespace SportBids.Api.Contracts.LeaderBoards.Create;

public class LeaderBoardCreateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string JoinCode { get; set; }
}
