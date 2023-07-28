using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;

namespace SportBids.Application.Authentication.Commands.SignIn;

public record SignInCommand(
    string UserName,
    string Password) : IRequest<Result<AuthResult>>;
