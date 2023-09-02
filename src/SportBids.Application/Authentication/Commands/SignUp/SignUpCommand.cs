using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;

namespace SportBids.Application.Authentication.Commands.SignUp;

public class SignUpCommand : IRequest<Result<AuthResult>>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string IPAddress { get; set; } = null!;
}
