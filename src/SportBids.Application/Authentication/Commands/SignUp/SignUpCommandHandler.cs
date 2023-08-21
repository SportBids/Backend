using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Commands.SendEmailConfirmation;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;
using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<AuthResult>>
{
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly IJwtFactory _jwtFactory;
    private readonly ISender _mediatr;

    public SignUpCommandHandler(IJwtFactory jwtFactory, IAuthService authService, IMapper mapper, ISender mediatr)
    {
        _jwtFactory = jwtFactory;
        _authService = authService;
        _mapper = mapper;
        _mediatr = mediatr;
    }

    public async Task<Result<AuthResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<AppUser>(request);
        var createUserResponse = await _authService.Create(user, request.Password);
        if (createUserResponse.IsFailed)
        {
            return Result.Fail<AuthResult>(createUserResponse.Errors);
        }

        var createdUser = createUserResponse.Value;

        var response = _mapper.Map<AuthResult>(createdUser);

        var refreshToken = _jwtFactory.GenerateRefreshToken(request.IPAddress);
        user.RefreshTokens.Add(refreshToken);

        response.AccessToken = _jwtFactory.GenerateAccessToken(createdUser.Id);
        response.RefreshToken = refreshToken.Token;

        await _authService.UpdateAsync(user);

        SendEmailConfirmation(createdUser);

        return Result.Ok(response);
    }

    private void SendEmailConfirmation(AppUser createdUser)
    {
        var command = new SendEmailConfirmationCommand()
        {
            User = createdUser
        };
        _mediatr.Send(command);
    }
}
