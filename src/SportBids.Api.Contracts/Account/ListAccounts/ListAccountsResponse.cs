using SportBids.Api.Contracts.Account.ListAccounts;

namespace SportBids.Api.Contracts;

public class ListAccountsResponse
{
    public IEnumerable<AccountDto> Accounts { get; set; }
}
