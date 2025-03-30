namespace BilvisionTennis.TennisGame;

public class Umpire
{
    public required string Name { get; set; }
    public Game? Game { get; set; }

    public void CalculateScore(Player winner, Player loser)
    {
        winner.Score++;

        if (winner.Score == 4 && loser.Score == 4)
        {
            winner.Score--;
            loser.Score--;
        }

        if (winner.Score >= 4 && winner.Score - loser.Score >= 2)
        {
            Game.GameHasWinner = true;
        }
        
        SetGameScore();
    }

    private void SetGameScore()
    {
        if (Game.GameHasWinner)
        {
            Game.Score = GetWinningScore();
        }
        else if (Game.PlayerOne.Score == Game.PlayerTwo.Score)
        {
            Game.Score = GetDrawScore();
        }
        else
        {
            Game.Score = GetScore();
        }
    }

    private string GetScore()
    {
        if (Game.PlayerOne.IsServer)
        {
            return $"{(Point)Game.PlayerOne.Score} - {(Point)Game.PlayerTwo.Score}";
        }
        
        return $"{(Point)Game.PlayerTwo.Score} - {(Point)Game.PlayerOne.Score}";
    }

    private string GetWinningScore()
    {
        Point playerOnePoint = (Point)Game.PlayerOne.Score;
        Point playerTwoPoint = (Point)Game.PlayerTwo.Score;
        
        if (Game.PlayerOne.Score > Game.PlayerTwo.Score)
        {
            playerOnePoint = Point.Game;
        }
        else
        {
            playerTwoPoint = Point.Game;
        }
        
        if (Game.PlayerOne.IsServer)
        {
            return $"{playerOnePoint} - {playerTwoPoint}";
        }
        
        return $"{playerTwoPoint} - {playerOnePoint}";
        
    }
    private string GetDrawScore()
    {
        return Game.PlayerOne.Score switch
        {
            1 => $"{Point.Fifteen} - All",
            2 => $"{Point.Thirty} - All",
            _ => $"{Point.Deuce}"
        };
    }

    public void DecideServerByCoinToss()
    {
        var random = new Random();
        string coinTossResult = random.Next(100) < 50 ? "heads" : "tails";

        if (coinTossResult == "heads")
        {
            Game.PlayerOne.IsServer = true;
        }
        else
        {
            Game.PlayerTwo.IsServer = true;
        }
    }
}