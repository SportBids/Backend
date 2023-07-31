﻿using FluentResults;
using MapsterMapper;
using MediatR;
using SportBids.Application.Authentication.Commands.SendEmailConfirmation;
using SportBids.Application.Authentication.Common;
using SportBids.Application.Interfaces.Authentication;
using SportBids.Application.Interfaces.Persistence;
using SportBids.Domain.Models;

namespace SportBids.Application.Authentication.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<AuthResult>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtFactory _jwtFactory;
    private readonly ISender _mediatr;

    public SignUpCommandHandler(IJwtFactory jwtFactory, IUserRepository userRepository, IMapper mapper, ISender mediatr)
    {
        _jwtFactory = jwtFactory;
        _userRepository = userRepository;
        _mapper = mapper;
        _mediatr = mediatr;
    }

    public async Task<Result<AuthResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var createUserResponse = await _userRepository.Create(user, request.Password);
        if (createUserResponse.IsFailed)
        {
            return Result.Fail<AuthResult>(createUserResponse.Errors);
        }

        var createdUser = createUserResponse.Value;

        var response = _mapper.Map<AuthResult>(createdUser);
        response.AccessToken = _jwtFactory.GenerateAccessToken(createdUser.Id);
        response.RefreshToken = _jwtFactory.GenerateRefreshToken();

        SendEmailConfirmation(createdUser);

        return Result.Ok(response);
    }

    private void SendEmailConfirmation(User createdUser)
    {
        var command = new SendEmailConfirmationCommand()
        {
            User = createdUser
        };
        _mediatr.Send(command);
    }
}
