using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportBids.Api.Contracts.Tournament.CreateGroupMatches;
using SportBids.Api.Contracts.Tournament.CreateMatch;
using SportBids.Api.Contracts.Tournament.CreateTournament;
using SportBids.Api.Contracts.Tournament.DeleteKnockoutMatch;
using SportBids.Api.Contracts.Tournament.GetTournament;
using SportBids.Api.Contracts.Tournament.GetTournaments;
using SportBids.Api.Contracts.Tournament.UpdateGroupTeams;
using SportBids.Api.Contracts.Tournament.UpdateMatch;
using SportBids.Api.Contracts.Tournament.UpdateTeam;
using SportBids.Api.Contracts.Tournament.UpdateTournament;
using SportBids.Application;
using SportBids.Application.Tournaments.Commands.CreateGroupMatches;
using SportBids.Application.Tournaments.Commands.CreateMatch;
using SportBids.Application.Tournaments.Commands.CreateTournament;
using SportBids.Application.Tournaments.Commands.DeleteKnockoutMatch;
using SportBids.Application.Tournaments.Commands.DeleteTournament;
using SportBids.Application.Tournaments.Commands.UpdateGroupTeams;
using SportBids.Application.Tournaments.Commands.UpdateTournament;
using SportBids.Application.Tournaments.Queries.GetTournaments;

namespace SportBids.Api.Controllers;

[Route("api/[controller]")]
public class TournamentsController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediatr;

    public TournamentsController(IMapper mapper,
                                ISender mediatr)
    {
        _mapper = mapper;
        _mediatr = mediatr;
    }

    /// <summary>
    /// Get all tournaments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTournamentsQuery();
        var tournaments = await _mediatr.Send(query, cancellationToken);
        var response = new GetTournamentsResponse
        {
            Tournaments = _mapper.Map<IEnumerable<GetTournamentsTournamentDto>>(tournaments)
        };
        return Ok(response);
    }

    /// <summary>
    /// Create tournament
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTournamentRequest request,
                                            CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateTournamentCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);

        if (response.IsFailed)
            return ProcessError(response.Errors);
        return StatusCode(StatusCodes.Status201Created, response.Value);
    }

    /// <summary>
    /// Get full tournament info with groups, teams, matches
    /// </summary>
    [HttpGet("{tournamentId}")]
    public async Task<IActionResult> GetTournamentFullInfo([FromRoute] GetTournamentRequest request,
                                                           CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetTournamentQuery>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        if (result.IsFailed)
        {
            return ProcessError(result.Errors);
        }
        var tournament = _mapper.Map<GetTournamentResponse>(result.Value);
        return Ok(tournament);
    }

    [HttpPut("{tournamentId}")]
    public async Task<IActionResult> Update([FromRoute] Guid tournamentId,
                                            [FromBody] UpdateTournamentRequest request,
                                            CancellationToken cancellationToken)
    {
        if (!tournamentId.Equals(request.Id)) return BadRequest();

        var command = _mapper.Map<UpdateTournamentCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return Ok(response.Value);
    }

    [HttpDelete("{tournamentId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid tournamentId,
                                            CancellationToken cancellationToken)
    {
        var command = new DeleteTournamentCommand { Id = tournamentId };
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return NoContent();
    }

    [HttpPut("{tournamentId}/groups/{groupId}")]
    public async Task<IActionResult> UpdateTeamsInGroup([FromRoute] Guid tournamentId,
                                                    [FromRoute] Guid groupId,
                                                    [FromBody] UpdateGroupTeamsRequest request,
                                                    CancellationToken cancellationToken)
    {
        if (tournamentId != request.TournamentId || groupId != request.GroupId)
            return BadRequest();
        var command = _mapper.Map<UpdateGroupTeamsCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return NoContent();
    }

    [HttpPut("{tournamentId}/teams/{teamId}")]
    public async Task<IActionResult> UpdateTeam([FromRoute] Guid tournamentId,
                                                [FromRoute] Guid teamId,
                                                [FromBody] UpdateTeamRequest request,
                                                CancellationToken cancellationToken)
    {
        if (tournamentId != request.TournamentId || teamId != request.TeamId)
            return BadRequest();
        var command = _mapper.Map<UpdateTeamCommand>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : ProcessError(result.Errors);
    }

    [HttpPut("{tournamentId}/matches/{matchId}")]
    public async Task<IActionResult> UpdateMatch([FromRoute] Guid tournamentId,
                                                [FromRoute] Guid matchId,
                                                [FromBody] UpdateMatchRequest request,
                                                CancellationToken cancellationToken)
    {
        if (tournamentId != request.TournamentId || matchId != request.MatchId)
            return BadRequest();
        var command = _mapper.Map<UpdateMatchCommand>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : ProcessError(result.Errors);
    }

    [HttpPost("{tournamentId}/createGroupMatches")]
    public async Task<IActionResult> CreateGroupMatches([FromRoute] Guid tournamentId,
                                                        [FromBody] CreateGroupMatchesRequest request,
                                                        CancellationToken cancellationToken)
    {
        if (tournamentId != request.TournamentId)
            return BadRequest();
        var command = _mapper.Map<CreateGroupMatchesCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("{tournamentId}/matches")]
    public async Task<IActionResult> CreateMatch([FromRoute] Guid tournamentId,
                                                 [FromBody] CreateMatchRequest request,
                                                 CancellationToken cancellationToken)
    {
        if (tournamentId != request.TournamentId) return BadRequest();

        var command = _mapper.Map<CreateMatchCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("{tournamentId}/matches/{matchId}")]
    public async Task<IActionResult> DeleteKnockoutMatch([FromRoute] DeleteKnockOutMatchRequest request,
                                                         CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteKnockOutMatchCommand>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : ProcessError(result.Errors);
    }
}
