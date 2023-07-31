using System.Reflection.Metadata;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportBids.Application.Authentication.Commands.ConfirmEmail;
using SportBids.Contracts.Account.Requests;

namespace SportBids.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediatr;

    public AccountController(IMapper mapper, ISender mediatr)
    {
        _mapper = mapper;
        _mediatr = mediatr;
    }

    [AllowAnonymous]
    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ConfirmEmailCommand>(request);
        var response = await _mediatr.Send(command, cancellationToken);
        if (response.IsFailed)
        {
            return BadRequest("Failed to confirm email!");
        }

        return Ok("Email confirmed successfuly.");
    }
}
