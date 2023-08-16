#nullable disable

using MediatR;
using SportBids.Domain.Models;

namespace SportBids.Application.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationCommand : IRequest
{
    public User User { get; set; }
}
