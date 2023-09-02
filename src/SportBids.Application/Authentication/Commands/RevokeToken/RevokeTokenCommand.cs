#nullable disable

using FluentResults;
using MediatR;

namespace SportBids.Application.Authentication.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public string IPAddress { get; set; }
}
