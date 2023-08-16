using System.Security.Claims;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportBids.Application.Accounts.Commands.ChangePassword;
using SportBids.Application.Accounts.Commands.EditAccount;
using SportBids.Application.Authentication.Commands.ConfirmEmail;
using SportBids.Contracts.Account.ChangePassword;
using SportBids.Contracts.Account.EditAccount;

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

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditAccountRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<EditAccountCommand>(request);
        command.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediatr.Send(command, cancellationToken);

        if (response.IsFailed)
        {
            return BadRequest("Failed to update.");
        }

        return Ok(_mapper.Map<EditAccountResponse>(response.Value));
    }

    [HttpPut("changePass")]
    public async Task<IActionResult> EditPassord([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ChangePasswordCommand>(request);
        command.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _mediatr.Send(command, cancellationToken);

        if (response.IsFailed)
        {
            return ProcessError(response.Errors);
        }

        return Ok();
    }
}
