using static BilvisionTennis.TennisGame.TennisPoints;

namespace BilvisionTennis.TennisGame;

public class Game
{
    public string WrittenScore { get; set; } = $"{WrittenGamePoints[0]} | {WrittenGamePoints[0]}";
    public string VerbalScore { get; set; } = $"{VerbalGamePoint.Love} | {VerbalGamePoint.Love}";
    public bool HasWinner { get; set; }
}