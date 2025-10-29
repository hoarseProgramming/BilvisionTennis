
namespace BilvisionTennis.Frontend.TennisGame;

using static BilvisionTennis.Frontend.TennisGame.TennisPoints;


public class Game
{
    public int Id { get; set; }
    public string WrittenScore { get; set; } = $"{WrittenGamePoints[0]} | {WrittenGamePoints[0]}";
    public string VerbalScore { get; set; } = $"{VerbalGamePoint.Love} | {VerbalGamePoint.Love}";
    public bool HasWinner { get; set; }
    public List<Rally> Rallies { get; set; } = [];
    public int Number { get; set; }
}