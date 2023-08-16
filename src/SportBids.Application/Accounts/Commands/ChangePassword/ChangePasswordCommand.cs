#nullable disable

using FluentResults;
using MediatR;

namespace SportBids.Application.Accounts.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
