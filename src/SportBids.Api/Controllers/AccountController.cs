using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportBids.Api.Contracts.Account.ChangePassword;
using SportBids.Api.Contracts.Account.EditAccount;
using SportBids.Api.Contracts.Account.ListAccounts;
using SportBids.Api.Contracts.Account.SetUserRole;
using SportBids.Application.Accounts.Commands.ChangePassword;
using SportBids.Application.Accounts.Commands.EditAccount;
using SportBids.Application.Accounts.Commands.SetUserRole;
using SportBids.Application.Accounts.Queries.GetAccounts;
using SportBids.Application.Authentication.Commands.ConfirmEmail;
using SportBids.Domain.Entities;

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
    public async Task<IActionResult> EditPassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
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

    [HttpGet]
    [Authorize(Policy = "adminOnly")]
    public async Task<IActionResult> ListUsers(CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery();
        var accounts = await _mediatr.Send(query, cancellationToken);
        var response = new ListAccountsResponse { Accounts = _mapper.Map<IEnumerable<AccountDto>>(accounts) };
        return Ok(response);
    }
    
    [HttpPut("{userId:Guid}")]
    [Authorize(Policy = "adminOnly")]
    public async Task<IActionResult> SetUserRole(Guid userId, UserRoleDto dto, CancellationToken cancellationToken)
    {
        UserRoles role = UserRoles.User;
        if (userId != dto.UserId || (!string.IsNullOrWhiteSpace(dto.Role) && !Enum.TryParse<UserRoles>(dto.Role, out role)))
            return BadRequest();
        
        var query = new SetUserRoleCommand { UserId = userId, Role = role };
        var result = await _mediatr.Send(query, cancellationToken);

        return result.IsSuccess ? Ok() : ProcessError(result.Errors);
    }
}
