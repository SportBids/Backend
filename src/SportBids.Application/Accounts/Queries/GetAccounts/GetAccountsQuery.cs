using MediatR;
using SportBids.Application.Common.Models;

namespace SportBids.Application.Accounts.Queries.GetAccounts;

public class GetAccountsQuery : IRequest<IList<AccountModel>>
{
}
