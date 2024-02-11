namespace SportBids.Api;

public interface IDBInitializer
{
    Task Initialize(IServiceProvider serviceProvider);
}
