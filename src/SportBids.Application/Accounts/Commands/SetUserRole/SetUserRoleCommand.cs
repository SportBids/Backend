﻿using FluentResults;
using MediatR;
using SportBids.Domain;

namespace SportBids.Application.Accounts.Commands.SetUserRole;

public class SetUserRoleCommand : IRequest<Result>
{
    public Guid UserId { get; init; }
    public UserRoles Role { get; init; }
}
