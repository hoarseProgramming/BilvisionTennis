using static BilvisionTennis.TennisGame.TennisPoints;

namespace BilvisionTennis.TennisGame;

public class Umpire
{
    public required string Name { get; set; }
    public TennisMatch? Match { get; set; }

    public void StartNewMatch()
    {
        DecideServerByCoinToss();
        StartNewSet(true);
    }
    public void StartNewSet(bool isFirstSetOfMatch = false)
    {
        Match.CurrentSet = new();

        Match.PlayerOne.GamesWon = 0;
        Match.PlayerTwo.GamesWon = 0;

        StartNewGame(isFirstSetOfMatch);
    }
    public void StartNewGame(bool isFirstGameOfMatch = false)
    {
        Match.CurrentSet.CurrentGame = new();

        Match.PlayerOne.PointsScored = 0;
        Match.PlayerTwo.PointsScored = 0;

        Match.PlayerOne.IsServer = !Match.PlayerOne.IsServer;
        Match.PlayerTwo.IsServer = !Match.PlayerTwo.IsServer;
    }
    public void DecideServerByCoinToss()
    {
        var random = new Random();
        string coinTossResult = random.Next(100) < 50 ? "heads" : "tails";

        if (coinTossResult == "heads")
        {
            Match.PlayerOne.IsServer = true;
        }
        else
        {
            Match.PlayerTwo.IsServer = true;
        }
    }
    public void ScorePointTo(int playerNumber)
    {
        if (Match.CurrentSet.CurrentGame.HasWinner)
        {
            return;
        }

        if (playerNumber == 1)
        {
            CalculateGameScore(winner: Match.PlayerOne, loser: Match.PlayerTwo);
        }
        else
        {
            CalculateGameScore(winner: Match.PlayerTwo, loser: Match.PlayerOne);
        }
    }

    public void CalculateGameScore(Player winner, Player loser)
    {
        winner.PointsScored++;

        if (winner.PointsScored == 4 && loser.PointsScored == 4)
        {
            winner.PointsScored--;
            loser.PointsScored--;
        }

        if (winner.PointsScored >= 4 && winner.PointsScored - loser.PointsScored >= 2)
        {
            Match.CurrentSet.CurrentGame.HasWinner = true;
            winner.GamesWon++;
            CalculateSetScore(winner, loser);
        }

        SetGameScore(winner, loser);
    }
    public void CalculateSetScore(Player winner, Player loser)
    {
        if (winner.GamesWon >= 6 && winner.GamesWon - loser.GamesWon >= 2)
        {
            Match.CurrentSet.HasWinner = true;
            winner.SetsWon++;
            CalculateMatchScore(winner, loser);
        }
    }
    public void CalculateMatchScore(Player winner, Player loser)
    {
        if (winner.SetsWon > Match.SetsInMatch / 2)
        {
            Match.Winner = winner;
        }
    }
    private void SetGameScore(Player winner, Player loser)
    {
        if (Match.CurrentSet.CurrentGame.HasWinner)
        {
            Match.CurrentSet.CurrentGame.WrittenScore = GetWinningScore(winner, loser);

            if (Match.Winner is null)
            {
                Match.CurrentSet.CurrentGame.VerbalScore = GetWinningScore(winner, loser, isWrittenScore: false);
            }
            else
            {
                Match.CurrentSet.CurrentGame.VerbalScore = $"The winner is {Match.Winner.Name}!";
            }
        }
        else if (Match.PlayerOne.PointsScored == Match.PlayerTwo.PointsScored)
        {
            Match.CurrentSet.CurrentGame.WrittenScore = GetScore();
            Match.CurrentSet.CurrentGame.VerbalScore = GetDrawScore();
        }
        else
        {
            Match.CurrentSet.CurrentGame.WrittenScore = GetScore();
            Match.CurrentSet.CurrentGame.VerbalScore = GetScore(isWrittenScore: false);
        }
    }
    private string GetWinningScore(Player winner, Player loser, bool isWrittenScore = true)
    {
        string writtenGamePointForPlayerOne = WrittenGamePoints[Match.PlayerOne.PointsScored];
        string writtenGamePointForPlayerTwo = WrittenGamePoints[Match.PlayerTwo.PointsScored];

        VerbalGamePoint verbalPointForPlayerOne = (VerbalGamePoint)Match.PlayerOne.PointsScored;
        VerbalGamePoint verbalPointForPlayerTwo = (VerbalGamePoint)Match.PlayerTwo.PointsScored;

        if (Match.PlayerOne.PointsScored > Match.PlayerTwo.PointsScored)
        {
            verbalPointForPlayerOne = VerbalGamePoint.Game;
            writtenGamePointForPlayerOne = WrittenGamePoints[5];
        }
        else
        {
            verbalPointForPlayerTwo = VerbalGamePoint.Game;
            writtenGamePointForPlayerTwo = WrittenGamePoints[5];
        }

        if (isWrittenScore)
        {
            return $"{writtenGamePointForPlayerOne} | {writtenGamePointForPlayerTwo}";
        }

        // Set right scoring order depending on who's the server.
        if (Match.PlayerOne.IsServer)
        {
            return $"{verbalPointForPlayerOne} | {verbalPointForPlayerTwo}";
        }

        return $"{verbalPointForPlayerTwo} | {verbalPointForPlayerOne}";
    }
    private string GetScore(bool isWrittenScore = true)
    {
        if (isWrittenScore)
        {
            return $"{WrittenGamePoints[Match.PlayerOne.PointsScored]} | {WrittenGamePoints[Match.PlayerTwo.PointsScored]}";
        }

        // Sets right scoring order depending on who's the server.

        if (Match.PlayerOne.IsServer)
        {
            return $"{(VerbalGamePoint)Match.PlayerOne.PointsScored} | {(VerbalGamePoint)Match.PlayerTwo.PointsScored}";
        }
        return $"{(VerbalGamePoint)Match.PlayerTwo.PointsScored} | {(VerbalGamePoint)Match.PlayerOne.PointsScored}";
    }
    private string GetDrawScore()
    {
        return Match.PlayerOne.PointsScored switch
        {
            1 => $"{VerbalGamePoint.Fifteen} - All",
            2 => $"{VerbalGamePoint.Thirty} - All",
            _ => $"{VerbalGamePoint.Deuce}"
        };
    }
}