namespace BilvisionTennis.TennisGame;

public class Player
{
    public required string Name { get; set; }
    public bool IsServer { get; set; }
    public int PointsScored { get; set; }
    public int GamesWon { get; set; }
    public int SetsWon { get; set; }
}