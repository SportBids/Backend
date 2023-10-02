﻿namespace SportBids.Domain;
public class Tournament : Entity<Guid>
{
    public string Name { get; set; }
    public string? Logo { get; set; }
    public bool IsPublic { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset FinishAt { get; set; }
    public ICollection<Team> Teams { get; set; }
    public ICollection<Group> Groups { get; set; }
    public ICollection<Match> KnockOutMatches { get; set; }

    public bool IsFinished => FinishAt < DateTimeOffset.Now;
}
