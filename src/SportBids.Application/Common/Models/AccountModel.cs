#nullable disable
namespace SportBids.Application.Common.Models;

public class AccountModel
{
    public Guid Id { get; init; }
    
    public string UserName { get; init; }
    
    public string Role { get; init; }
    
    public string FirstName { get; init; }

    public string LastName { get; init; }
}
