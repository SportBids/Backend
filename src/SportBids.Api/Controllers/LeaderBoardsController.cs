using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportBids.Api.Contracts.LeaderBoards.Create;
using SportBids.Api.Contracts.LeaderBoards.List;
using SportBids.Api.Contracts.LeaderBoards.Update;
using SportBids.Application.LeaderBoard.Commands.Create;
using SportBids.Application.LeaderBoard.Commands.Delete;
using SportBids.Application.LeaderBoard.Commands.Join;
using SportBids.Application.LeaderBoard.Commands.Leave;
using SportBids.Application.LeaderBoard.Commands.Update;
using SportBids.Application.LeaderBoard.Queries.GetLeaderBoard;
using SportBids.Application.LeaderBoard.Queries.GetOwningLeaderBoards;
using SportBids.Application.LeaderBoard.Queries.MemberOf;

namespace SportBids.Api.Controllers;

[Route("api/[Controller]")]
public class LeaderBoardsController : ApiControllerBase
{
    private readonly ISender _mediatr;
    private readonly IMapper _mapper;

    public LeaderBoardsController(ISender mediatr, IMapper mapper)
    {
        _mediatr = mediatr;
        _mapper  = mapper;
    }

    /// <summary>
    /// Get global leaderboard with members and points
    /// </summary>
    [HttpGet("tournament/{tournamentId:Guid}")]
    public async Task<IActionResult> GetPublicBoard([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
    {
        var query = new LeaderBoardQuery
        {
            TournamentId  = tournamentId,
            LeaderBoardId = default(Guid)
        };
        var response = await _mediatr.Send(query, cancellationToken);
        return Ok(_mapper.Map<LeaderBoardDto>(response));
    }

    /// <summary>
    /// Create
    /// </summary>
    [HttpPost("private")]
    public async Task<IActionResult> Create(
        LeaderBoardCreateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LeaderBoardCreateCommand()
        {
            OwnerId = GetUserId(),
            Name    = request.Name
        };
        var response = await _mediatr
            .Send(command, cancellationToken);
        return response.IsSuccess
            ? Ok(_mapper.Map<LeaderBoardCreateResponse>(response.Value))
            : ProcessError(response.Errors);
    }

    /// <summary>
    /// Get private leader board with members and points
    /// </summary>
    [HttpGet("private/{boardId:Guid}/tournament/{tournamentId:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid tournamentId, [FromRoute] Guid boardId, CancellationToken cancellationToken)
    {
        var query = new LeaderBoardQuery
        {
            TournamentId  = tournamentId,
            LeaderBoardId = boardId
        };
        var response = await _mediatr.Send(query, cancellationToken);
        return Ok(_mapper.Map<LeaderBoardDto>(response));
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPut("private/{boardId:Guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid boardId,
        LeaderBoardUpdateRequest request,
        CancellationToken cancellationToken)
    {
        if (boardId != request.Id) return BadRequest();
        var command = new LeaderBoardUpdateCommand()
        {
            InitiatorId   = GetUserId(),
            LeaderBoardId = boardId,
            Name          = request.Name
        };
        var response = await _mediatr.Send(command, cancellationToken);
        return response.IsSuccess
            ? Ok(_mapper.Map<LeaderBoardUpdateResponse>(response.Value))
            : ProcessError(response.Errors);
    }

    /// <summary>
    /// Delete 
    /// </summary>
    [HttpDelete("private/{boardId:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid boardId, CancellationToken cancellationToken)
    {
        var command = new LeaderBoardDeleteCommand()
        {
            InitiatorId = GetUserId(),
            BoardId     = boardId
        };
        var response = await _mediatr.Send(command, cancellationToken);
        return response.IsSuccess ? Ok() : ProcessError(response.Errors);
    }

    /// <summary>
    /// Join private leaderboard
    /// </summary>
    [HttpPut("private/join/{joinCode}")]
    public async Task<IActionResult> Join(string joinCode, CancellationToken cancellationToken)
    {
        var command = new JoinPrivateLeaderBoardCommand()
        {
            JoinCode = joinCode,
            UserId   = GetUserId()
        };
        var response = await _mediatr.Send(command, cancellationToken);
        return response.IsSuccess ? Ok() : ProcessError(response.Errors);
    }
    
    /// <summary>
    /// Leave private leaderboard
    /// </summary>
    [HttpPut("private/{boardId:Guid}/leave")]
    public async Task<IActionResult> Leave(Guid boardId, CancellationToken cancellationToken)
    {
        var command = new LeavePrivateLeaderBoardCommand()
        {
            BoardId = boardId,
            UserId   = GetUserId()
        };
        var response = await _mediatr.Send(command, cancellationToken);
        return response.IsSuccess ? Ok() : ProcessError(response.Errors);
    }

    /// <summary>
    /// Returns a list of private leaderboard where requester is member
    /// </summary>
    [HttpGet("private")]
    public async Task<IActionResult> GetPrivateMembership(CancellationToken cancellationToken)
    {
        var query    = new PrivateLeaderBoardsMemberOfQuery() { UserId = GetUserId() };
        var boards = await _mediatr.Send(query, cancellationToken);
        return Ok(_mapper.Map<PrivateLeaderBoardShortDto>(boards));
    }
    
    /// <summary>
    /// Fetch own private leaderboards
    /// </summary>
    [HttpGet("private/own")]
    public async Task<IActionResult> GetOwnPrivateLeaderboards(CancellationToken cancellationToken)
    {
        var query  = new OwningLeaderBoardsQuery() { OwnerId = GetUserId() };
        var boards = await _mediatr.Send(query, cancellationToken);
        return Ok(_mapper.Map<PrivateLeaderBoardShortDto>(boards));
    }
}
