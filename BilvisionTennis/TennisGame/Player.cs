namespace BilvisionTennis.Frontend.TennisGame;

public class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsServer { get; set; }
    public int PointsScored { get; set; }
    public int GamesWon { get; set; }
    public int SetsWon { get; set; }

    public override string ToString() => Name;
}