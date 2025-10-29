
namespace BilvisionTennis.Frontend.TennisGame;

public class Set
{
    public int Id { get; set; }
    public bool HasWinner { get; set; }
    public List<Game> Games { get; set; } = [];
    public int Number { get; set; }
    public Game CurrentGame { get; set; }
}

