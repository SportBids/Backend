using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportBids.Api.Contracts.MatchPrediction;
using SportBids.Api.Contracts.MatchPrediction.CreateUpdate;
using SportBids.Api.Contracts.MatchPrediction.Get;
using SportBids.Application.MatchPredictions.Commands.CreateUpdate;
using SportBids.Application.MatchPredictions.Queries.GetMatchesPredictions;
using SportBids.Application.MatchPredictions.Queries.GetMatchesWithPredictions;
using SportBids.Domain.Entities;

namespace SportBids.Api.Controllers;

[Route("api/[Controller]")]
public class PredictionsController : ApiControllerBase
{
    private readonly ISender _mediatr;
    private readonly IMapper _mapper;

    public PredictionsController(ISender mediatr, IMapper mapper)
    {
        _mediatr = mediatr;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetMatchesWithPrediction(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            return BadRequest();
        var query = new MatchesWithPredictionQuery { UserId = userId };
        var result = await _mediatr.Send(query, cancellationToken);
        return Ok(_mapper.Map<IEnumerable<MatchWithPredictionDto>>(result.Value));
    }

    [HttpPut]
    public async Task<IActionResult> Get(
        [FromBody] GetPredictionsRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid requesterId))
            return BadRequest();

        // TODO: check if provided OwnerId exists
        if (request.OwnerId != null && requesterId != request.OwnerId && !User.HasClaim(ClaimTypes.Role, "admin"))
        {
            return Forbid();
        }

        var query = new MatchPredictionsQuery()
        {
            MatchIds = request.MatchIds,
            OwnerId = request.OwnerId ?? requesterId
        };
        var result = await _mediatr.Send(query, cancellationToken);
        return result.IsSuccess
            ? Ok(new GetPredictionsResponse { Predictions = _mapper.Map<IEnumerable<PredictionDto>>(result.Value) })
            : ProcessError(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(
        [FromBody] CreateUpdatePredictionsRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid createdById))
            return BadRequest();

        // TODO: check if provided OwnerId exists
        var command = new CreateUpdateMatchPredictionsCommand()
        {
            Predictions = _mapper.Map<Prediction[]>(request.Predictions),
            CreatedById = createdById,
            OwnerId = request.OwnerId ?? createdById
        };
        var result = await _mediatr.Send(command, cancellationToken);
        return result.IsSuccess
            ? Created()
            : ProcessError(result.Errors);
    }
}
