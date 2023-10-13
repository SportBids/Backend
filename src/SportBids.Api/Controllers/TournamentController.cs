using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportBids.Api.Contracts.Tournament.CreateTournament;
using SportBids.Api.Contracts.Tournament.UpdateTournament;
using SportBids.Application.Tournaments.Commands.CreateTournament;
using SportBids.Application.Tournaments.Commands.DeleteTournament;
using SportBids.Application.Tournaments.Commands.UpdateTournament;
using SportBids.Application.Tournaments.Queries;

namespace SportBids.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediatr;

    public TournamentController(IMapper mapper,
                                ISender mediatr)
    {
        _mapper = mapper;
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTournamentsQuery();
        var response = await _mediatr.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTournamentRequest request,
                                            CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateTournamentCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);

        if (response.IsFailed)
            return ProcessError(response.Errors);
        return Ok(response.Value);
    }

    [HttpPut("{tournamentId}")]
    public async Task<IActionResult> Update([FromQuery] Guid tournamentId,
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
    public async Task<IActionResult> Delete(Guid tournamentId,
                                            CancellationToken cancellationToken)
    {
        var command = new DeleteTournamentCommand { Id = tournamentId };
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
            return ProcessError(response.Errors);
        return Ok();
    }
}
