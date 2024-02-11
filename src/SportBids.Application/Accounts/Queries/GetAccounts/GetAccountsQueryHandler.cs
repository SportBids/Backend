using MediatR;
using SportBids.Application.Common.Models;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IList<AccountModel>>
{
    private readonly IAuthService _authService;

    public GetAccountsQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IList<AccountModel>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var users = await _authService
            .GetUsersAsync(cancellationToken);
        return users
            .Where(u => u.UserName != "administrator")
            .ToList();
    }
}
