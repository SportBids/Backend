namespace SportBids.Domain;

public struct Score
{
    public Score(int team1, int team2)
    {
        Team1 = team1;
        Team2 = team2;
    }
    public int Team1 { get; }
    public int Team2 { get; }
}
