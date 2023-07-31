using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using MediatR;

namespace SportBids.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}
