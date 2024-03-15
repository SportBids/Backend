using FluentResults;
using FluentValidation.AspNetCore;
using MediatR;

namespace SportBids.Application.LeaderBoard.Commands.Delete;

public class LeaderBoardDeleteCommand : IRequest<Result>
{
    public Guid InitiatorId { get; set; }
    public Guid BoardId { get; set; }
}
