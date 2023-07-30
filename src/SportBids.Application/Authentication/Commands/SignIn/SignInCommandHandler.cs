using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Common.Errors;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Persistence;

namespace SportBids.Application.Authentication.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<AuthResult>>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SignInCommandHandler(IUserRepository userRepository, IJwtFactory jwtFactory, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtFactory = jwtFactory;
        _mapper = mapper;
    }

    public async Task<Result<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserIfValidPassword(request.UserName, request.Password);
        if (user is null)
            return Result.Fail<AuthResult>(new SignInError());

        var response = _mapper.Map<AuthResult>(user);
        response.AccessToken = _jwtFactory.GenerateAccessToken(user.Id);
        response.RefreshToken = _jwtFactory.GenerateRefreshToken();

        return Result.Ok(response);
    }
}
