namespace BilvisionTennis.TennisGame;

public static class TennisPoints
{
    public static Dictionary<int, string> WrittenGamePoints = new()
    {
        { 0, "Love" },
        { 1, "15" },
        { 2, "30" },
        { 3, "40" },
        { 4, "Advantage" },
        { 5, "Game" },
    };

    public enum VerbalGamePoint
    {
        Love,
        Fifteen,
        Thirty,
        Forty,
        Advantage,
        Deuce,
        Game
    }
}

