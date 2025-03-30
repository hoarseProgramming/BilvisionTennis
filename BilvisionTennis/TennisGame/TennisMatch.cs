
namespace BilvisionTennis.TennisGame;

public class TennisMatch
{
    public Player PlayerOne { get; }
    public Player PlayerTwo { get; }
    public Umpire Umpire { get; }
    public Player? Winner { get; set; }
    public Set CurrentSet { get; set; }
    public int SetsInMatch { get; }

    public TennisMatch(string playerOneName, string playerTwoName, string umpireName, int setsInMatch)
    {
        PlayerOne = new() { Name = playerOneName };
        PlayerTwo = new() { Name = playerTwoName };
        Umpire = new() { Name = umpireName };
        Umpire.Match = this;
        SetsInMatch = setsInMatch;
        Umpire.StartNewMatch();
    }

    public string Score()
    {
        return $"Players:{PlayerOne.Name}|{PlayerTwo.Name}\n" +
            $"Game score:{CurrentSet.CurrentGame.WrittenScore}\n" +
            $"Games won:{PlayerOne.GamesWon}|{PlayerTwo.GamesWon}\n" +
            $"Sets won:{PlayerOne.SetsWon}|{PlayerTwo.SetsWon}";
    }
}
