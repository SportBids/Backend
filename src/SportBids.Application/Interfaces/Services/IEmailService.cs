namespace SportBids.Application.Interfaces.Services;

public interface IEmailService
{
    public Task SendAsync(string subject, string body, string[] to, CancellationToken cancellationToken);
}
