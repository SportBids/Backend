using System.Net;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportBids.Application.Authentication.Commands.RefreshToken;
using SportBids.Application.Authentication.Commands.RevokeToken;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Application.Authentication.Queries.SignIn;
using SportBids.Contracts.Account.SignUp;
using SportBids.Contracts.Authentication.RefreshToken;
using SportBids.Contracts.Authentication.SignIn;
using SportBids.Contracts.Authentication.SignOut;

namespace SportBids.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediatr;

    public AuthenticationController(IMapper mapper, ISender mediatr)
    {
        _mapper = mapper;
        _mediatr = mediatr;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SignUpCommand>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            var response = _mapper.Map<SignUpResponse>(result.Value);
            return Ok(response);
        }

        return ProcessError(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SignInCommand>(request);
        var result = await _mediatr.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Ok(_mapper.Map<SignInResponse>(result.Value));

        return ProcessError(result.Errors);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RefreshTokenCommand>(request);
        command.IPAddress = GetRemoteIpAddress() ?? "unknown IP";
        var result = await _mediatr.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return ProcessError(result.Errors);
    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOut([FromBody] SignOutRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RevokeTokenCommand>(request);
        command.IPAddress = GetRemoteIpAddress() ?? "unknown IP";
        await _mediatr.Send(command, cancellationToken);
        return Ok();
    }

    private string? GetRemoteIpAddress()
    {
        const string xForwardedFor = "X-Forwarded-For";
        if (Request.Headers.ContainsKey(xForwardedFor))
            return Request.Headers[xForwardedFor]!;

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
