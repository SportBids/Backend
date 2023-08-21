using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;

namespace SportBids.Application.Authentication.Queries.SignIn;

public class SignInCommand : IRequest<Result<AuthResult>>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string IPAddress { get; set; } = null!;
}
