namespace BilvisionTennis.Frontend.TennisGame;

public class TennisMatch
{
    public int Id { get; set; }
    public Player PlayerOne { get; }
    public Player PlayerTwo { get; }
    public Umpire Umpire { get; }
    public Player? Winner { get; set; }
    public Set CurrentSet { get; set; }
    public int SetsInMatch { get; }

    public TennisMatch(Player playerOne, Player playerTwo, Umpire umpire, int setsInMatch, int matchId)
    {
        Id = matchId;
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Umpire = umpire;
        Umpire.Match = this;
        SetsInMatch = setsInMatch;
    }

    public string Score()
    {
        return $"Players:{PlayerOne.Name}|{PlayerTwo.Name}\n" +
            $"Game score:{CurrentSet.CurrentGame.WrittenScore}\n" +
            $"Games won:{PlayerOne.GamesWon}|{PlayerTwo.GamesWon}\n" +
            $"Sets won:{PlayerOne.SetsWon}|{PlayerTwo.SetsWon}";
    }
}
