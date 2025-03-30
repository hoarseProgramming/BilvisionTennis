namespace BilvisionTennis.TennisGame;

public class Game(Player playerOne, Player playerTwo, Umpire umpire, bool isFirstGameInSet = false)
{
    public Player PlayerOne { get; } = playerOne;
    public Player PlayerTwo { get; } = playerTwo;
    public Umpire Umpire { get; } = umpire;
    public string Score { get; set; } = $"{Point.Love} - {Point.Love}";
    private readonly bool _isFirstGameInSet = isFirstGameInSet;
    private readonly Random _random = new Random();
    public bool GameHasWinner { get; set; }

    private void SetupGame()
    {
        Umpire.Game = this;
        
        if (_isFirstGameInSet)
        {
            Umpire.DecideServerByCoinToss();
        }
        else
        {
            PlayerOne.IsServer = !PlayerOne.IsServer;
            PlayerTwo.IsServer = !PlayerTwo.IsServer;
        }
    }

    public void ScorePointTo(int playerNumber)
    {
        if (playerNumber == 1)
        {
            Umpire.CalculateScore(winner: PlayerOne, loser: PlayerTwo);
        }
        else
        {
            Umpire.CalculateScore(winner: PlayerTwo, loser: PlayerOne);
        }
    }
}