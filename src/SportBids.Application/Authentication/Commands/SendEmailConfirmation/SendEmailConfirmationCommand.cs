#nullable disable

using MediatR;
using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Commands.SendEmailConfirmation;

public class SendEmailConfirmationCommand : IRequest
{
    public AppUser User { get; set; }
}
