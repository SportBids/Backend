using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportBids.Application.Interfaces.Services;

namespace SportBids.Infrastructure.Services;

public class EmailService : IEmailService

{
    public Task SendAsync(string subject, string body, string[] to, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("I am email service");
        System.Console.WriteLine($"Subject: {subject}");
        System.Console.WriteLine($"Body: {body}");
        System.Console.WriteLine($"To: {string.Join(", ", to)}");
        return Task.CompletedTask;
    }
}
