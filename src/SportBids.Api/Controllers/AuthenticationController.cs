using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportBids.Application.Authentication.Commands.SignIn;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Contracts.Authentication.Requests;
using SportBids.Contracts.Authentication.Responses;

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
    
    [HttpPost("signout")]
    public Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
