using static BilvisionTennis.TennisGame.TennisPoints;

namespace BilvisionTennis.TennisGame;

public class Game
{
    //public Player PlayerOne { get; }
    //public Player PlayerTwo { get; }
    //public Umpire Umpire { get; }
    public string WrittenScore { get; set; } = $"{WrittenGamePoints[0]} | {WrittenGamePoints[0]}";
    public string VerbalScore { get; set; } = $"{VerbalGamePoint.Love} | {VerbalGamePoint.Love}";
    public bool HasWinner { get; set; }
    //public Game(Player playerOne, Player playerTwo, Umpire umpire)
    //{
    //    PlayerOne = playerOne;
    //    PlayerTwo = playerTwo;
    //    Umpire = umpire;
    //}
}