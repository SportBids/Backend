#nullable disable

using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;

namespace SportBids.Application.Authentication.Commands.RenewJwt;

public class RenewJwtCommand : IRequest<Result<AuthResult>>
{
    public string RefreshToken { get; set; }
    public string IPAddress { get; set; }
}
