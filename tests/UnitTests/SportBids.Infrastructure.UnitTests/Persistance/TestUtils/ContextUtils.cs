using Microsoft.EntityFrameworkCore;
using SportBids.Domain;
using SportBids.Infrastructure.Persistence;

namespace SportBids.Infrastructure.UnitTests;

public class ContextUtils
{
    public static AppDbContext CreateAppDbContext()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase("InMemoryForUnitTests")
            .Options;
        return new AppDbContext(options);
    }

    public static void CreateTournament(AppDbContext context)
    {
        var tournament = new Tournament()
        {
            Name = "Test Tournament Name",
            StartAt = DateTimeOffset.UtcNow,
            FinishAt = DateTimeOffset.UtcNow.AddDays(10),
            Groups = new Group[] {
                new Group{Name="A" }
            },
            Teams = new Team[] {
                new Team { Name = "Team 01" },
                new Team { Name = "Team 02" },
            }
        };

        foreach (Group group in tournament.Groups)
        {
            group.Teams = new Team[] { tournament.Teams.First(), tournament.Teams.Last() };
            group.Matches = new Match[]
            {
                new Match {Team1 = group.Teams.First(), Team2 = group.Teams.Last() }
            };
        }
        context.Set<Tournament>().Add(tournament);
        context.SaveChanges();
    }

    public static Prediction CreatePrediction(AppDbContext context)
    {
        var tournament = context.Set<Tournament>().First();
        var match = tournament.Groups.First().Matches.First();
        var ownerId = Guid.NewGuid();
        var prediction = new Prediction
        {
            OwnerId = ownerId,
            Owner = new Domain.Entities.AppUser { Id = ownerId, Email = "no@mail.ru", UserName = "no" },
            MatchId = match.Id,
            Score = new Score(1, 2)
        };
        context.Set<Prediction>().Add(prediction);
        context.SaveChanges();
        return prediction;
    }
}
