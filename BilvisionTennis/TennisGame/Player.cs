namespace BilvisionTennis.TennisGame;

public class Player
{
    public required string Name { get; set; }
    public int WinProbabilityInPercent { get; set; }
    public bool IsServer { get; set; }
    public int Score { get; set; }
}