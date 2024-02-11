#nullable disable
namespace SportBids.Api.Contracts.Account.ListAccounts;

public class ListAccountsResponse
{
    public IEnumerable<AccountDto> Accounts { get; set; }
}
