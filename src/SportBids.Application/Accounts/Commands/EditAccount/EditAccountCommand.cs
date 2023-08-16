#nullable disable

using FluentResults;
using MediatR;
using SportBids.Application.Authentication.Common;
using SportBids.Domain.Models;

namespace SportBids.Application.Accounts.Commands.EditAccount;

public class EditAccountCommand : IRequest<Result<User>>
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
