using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.Authentication.Queries.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<AuthResult>>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public SignInCommandHandler(IAuthService authService, IJwtFactory jwtFactory, IMapper mapper)
    {
        _authService = authService;
        _jwtFactory = jwtFactory;
        _mapper = mapper;
    }

    public async Task<Result<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.GetUserIfValidPassword(request.UserName, request.Password);
        if (user is null)
            return Result.Fail<AuthResult>(new SignInError());

        var response = _mapper.Map<AuthResult>(user);

        var refreshToken = _jwtFactory.GenerateRefreshToken(request.IPAddress);
        user.RefreshTokens.Add(refreshToken);

        response.AccessToken = _jwtFactory.GenerateAccessToken(user.Id);
        response.RefreshToken = refreshToken.Token;

        await _authService.UpdateAsync(user);

        return Result.Ok(response);
    }
}
