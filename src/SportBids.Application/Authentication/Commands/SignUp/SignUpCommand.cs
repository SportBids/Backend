using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;

namespace SportBids.Application.Authentication.Commands.SignUp;

public record SignUpCommand(
    string UserName,
    string Password,
    string Email,
    string FirstName,
    string LastName) : IRequest<Result<AuthResult>>;
