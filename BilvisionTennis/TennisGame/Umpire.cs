

namespace BilvisionTennis.Frontend.TennisGame;

using BilvisionTennis.Frontend.GraphQl;
using static BilvisionTennis.Frontend.TennisGame.TennisPoints;


public class Umpire
{
    private readonly ITennisClient _tennisClient;
    public int Id { get; set; }
    public required string Name { get; set; }
    public TennisMatch? Match { get; set; }
    public Umpire(ITennisClient tennisClient)
    {
        _tennisClient = tennisClient;
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
    public async Task StartNewMatch(int gameId, int setId)
    {
        DecideServerByCoinToss();
        await StartNewSet(true, setId, gameId);
    }
    public async Task StartNewSet( bool isFirstSetOfMatch = false, int? setId = null, int? gameId = null)
    {
        if(isFirstSetOfMatch)
        {
            Match.CurrentSet = new Set { 
                Id = (int)gameId
            };

            await StartNewGame(isFirstSetOfMatch, (int)setId);
        }
        else
        {
            var addGameResult = await _tennisClient.AddGame.ExecuteAsync(new AddGameInput
            {
                MatchId = Match.Id,
                Number = Match.PlayerOne.SetsWon + Match.PlayerTwo.SetsWon + 1
            });

            Set newSet = new Set { Id = addGameResult.Data.AddGame.Game.Id };
            Match.CurrentSet = newSet;
            await StartNewGame(isFirstSetOfMatch, newSet.Id);
        }

        Match.PlayerOne.GamesWon = 0;
        Match.PlayerTwo.GamesWon = 0;

    }
    public async Task StartNewGame(bool isFirstGameOfMatch = false, int? gameId = null)
    {
        if(isFirstGameOfMatch)
        {
            Match.CurrentSet.CurrentGame = new()
            {
                Id = (int)gameId
            };
        } else
        {
            var addSetResult = await _tennisClient.AddSet.ExecuteAsync(new AddSetInput
            {
                GameId = Match.CurrentSet.Id,
                Number = Match.PlayerOne.GamesWon + Match.PlayerTwo.GamesWon + 1
            });

            Match.CurrentSet.CurrentGame = new()
            {
                Id = addSetResult.Data.AddSet.Set.Id
            };
        }

            Match.PlayerOne.PointsScored = 0;
        Match.PlayerTwo.PointsScored = 0;

        if (!isFirstGameOfMatch)
        {
            Match.PlayerOne.IsServer = !Match.PlayerOne.IsServer;
            Match.PlayerTwo.IsServer = !Match.PlayerTwo.IsServer;
        }
    }

    public async Task ScorePointToPlayer(int playerNumber)
    {
        if (Match.CurrentSet.CurrentGame.HasWinner)
        {
            return;
        }

        int shotNumber = Match.PlayerOne.PointsScored + Match.PlayerTwo.PointsScored + 1;

        if (playerNumber == 1)
        {
            await _tennisClient.AddRally.ExecuteAsync(new AddRallyInput
            {
                ScorerId = Match.PlayerOne.Id,
                SetId = Match.CurrentSet.CurrentGame.Id,
                Number = shotNumber
            });
            Match.PlayerOne.PointsScored++;
            await CalculateGameScore(winner: Match.PlayerOne, loser: Match.PlayerTwo);
        }
        else
        {
            await _tennisClient.AddRally.ExecuteAsync(new AddRallyInput
            {
                ScorerId = Match.PlayerTwo.Id,
                SetId = Match.CurrentSet.CurrentGame.Id,
                Number = shotNumber
            });
            Match.PlayerTwo.PointsScored++;
            await CalculateGameScore(winner: Match.PlayerTwo, loser: Match.PlayerOne);
        }
    }

    public async Task CalculateGameScore(Player winner, Player loser)
    {
        if (winner.PointsScored == 4 && loser.PointsScored == 4)
        {
            winner.PointsScored--;
            loser.PointsScored--;
        }

        if (winner.PointsScored >= 4 && winner.PointsScored - loser.PointsScored >= 2)
        {
            Match.CurrentSet.CurrentGame.HasWinner = true;
            winner.GamesWon++;
            await CalculateSetScore(winner, loser);
        }

        SetGameScore(winner, loser);
    }
    public async Task CalculateSetScore(Player winner, Player loser)
    {
        if (winner.GamesWon >= 6 && winner.GamesWon - loser.GamesWon >= 2)
        {
            Match.CurrentSet.HasWinner = true;
            winner.SetsWon++;
            await CalculateMatchScore(winner, loser);
        }
    }
    public async Task CalculateMatchScore(Player winner, Player loser)
    {
        if (winner.SetsWon > Match.SetsInMatch / 2)
        {
            var updateWinnerResult = await _tennisClient.UpdateMatchWinner.ExecuteAsync(new UpdateMatchWinnerInput
            {
                PlayerId = winner.Id,
                MatchId = Match.Id
            });
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

    public override string ToString() => Name;
}