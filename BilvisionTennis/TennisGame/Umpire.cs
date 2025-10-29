

namespace BilvisionTennis.Frontend.TennisGame;

using BilvisionTennis.Frontend.GraphQl;
using static BilvisionTennis.Frontend.TennisGame.TennisPoints;


public class Umpire(ITennisClient _tennisClient, GlobalStore globalStore)
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public TennisMatch? CurrentMatch { get; set; }
    public List<TennisMatch> Matches { get; set; } = [];

    public void DecideServerByCoinToss()
    {
        var random = new Random();
        string coinTossResult = random.Next(100) < 50 ? "heads" : "tails";

        if (coinTossResult == "heads")
        {
            CurrentMatch.PlayerOne.IsServer = true;
        }
        else
        {
            CurrentMatch.PlayerTwo.IsServer = true;
        }
    }
    public async Task StartNewMatch(int gameId, int setId)
    {
        DecideServerByCoinToss();
        await StartNewSet(true, setId, gameId);
    }
    public async Task StartNewSet(bool isFirstSetOfMatch = false, int? setId = null, int? gameId = null)
    {
        CurrentMatch.PlayerOne.GamesWon = 0;
        CurrentMatch.PlayerTwo.GamesWon = 0;
        if (isFirstSetOfMatch)
        {
            CurrentMatch.CurrentSet = new Set
            {
                Id = (int)gameId
            };

            await StartNewGame(isFirstGameOfSet: true, setId: (int)setId);
        }
        else
        {
            var addGameResult = await _tennisClient.AddGame.ExecuteAsync(new AddGameInput
            {
                MatchId = CurrentMatch.Id,
                Number = CurrentMatch.PlayerOne.SetsWon + CurrentMatch.PlayerTwo.SetsWon + 1
            });

            Set newSet = new Set
            {
                Id = addGameResult.Data.AddGame.Game.Id,
                Games = addGameResult.Data?.AddGame.Game.Sets.Select(s => new Game
                {
                    Id = s.Id
                }).ToList()
            };
            CurrentMatch.CurrentSet = newSet;

            if (newSet.Games.Count == 1)
            {
                await StartNewGame(isFirstGameOfSet: true, setId: newSet.Games[0].Id);
            }
            else
            {
                await StartNewGame(gameId: newSet.Games[0].Id);
            }
        }
    }
    public async Task StartNewGame(bool isFirstGameOfSet = false, int? gameId = null, int? setId = null)
    {
        if (isFirstGameOfSet)
        {
            CurrentMatch.CurrentSet.CurrentGame = new()
            {
                Id = (int)setId
            };
        }
        else
        {
            var addSetResult = await _tennisClient.AddSet.ExecuteAsync(new AddSetInput
            {
                GameId = CurrentMatch.CurrentSet.Id,
                Number = CurrentMatch.PlayerOne.GamesWon + CurrentMatch.PlayerTwo.GamesWon + 1
            });

            CurrentMatch.CurrentSet.CurrentGame = new()
            {
                Id = addSetResult.Data.AddSet.Set.Id
            };
        }

        CurrentMatch.PlayerOne.PointsScored = 0;
        CurrentMatch.PlayerTwo.PointsScored = 0;

        if (!isFirstGameOfSet)
        {
            CurrentMatch.PlayerOne.IsServer = !CurrentMatch.PlayerOne.IsServer;
            CurrentMatch.PlayerTwo.IsServer = !CurrentMatch.PlayerTwo.IsServer;
        }
    }

    public async Task ScorePointToPlayer(int playerNumber)
    {
        if (CurrentMatch.CurrentSet.CurrentGame.HasWinner)
        {
            return;
        }

        int shotNumber = CurrentMatch.PlayerOne.PointsScored + CurrentMatch.PlayerTwo.PointsScored + 1;

        if (playerNumber == 1)
        {
            await _tennisClient.AddRally.ExecuteAsync(new AddRallyInput
            {
                ScorerId = CurrentMatch.PlayerOne.Id,
                SetId = CurrentMatch.CurrentSet.CurrentGame.Id,
                Number = shotNumber
            });
            CurrentMatch.PlayerOne.PointsScored++;
            await CalculateGameScore(winner: CurrentMatch.PlayerOne, loser: CurrentMatch.PlayerTwo);
        }
        else
        {
            await _tennisClient.AddRally.ExecuteAsync(new AddRallyInput
            {
                ScorerId = CurrentMatch.PlayerTwo.Id,
                SetId = CurrentMatch.CurrentSet.CurrentGame.Id,
                Number = shotNumber
            });
            CurrentMatch.PlayerTwo.PointsScored++;
            await CalculateGameScore(winner: CurrentMatch.PlayerTwo, loser: CurrentMatch.PlayerOne);
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
            CurrentMatch.CurrentSet.CurrentGame.HasWinner = true;
            winner.GamesWon++;
            await CalculateSetScore(winner, loser);
        }

        SetGameScore(winner, loser);
    }
    public async Task CalculateSetScore(Player winner, Player loser)
    {
        if (winner.GamesWon >= 6 && winner.GamesWon - loser.GamesWon >= 2)
        {
            CurrentMatch.CurrentSet.HasWinner = true;
            winner.SetsWon++;
            await CalculateMatchScore(winner, loser);
        }
    }
    public async Task CalculateMatchScore(Player winner, Player loser)
    {
        if (winner.SetsWon > CurrentMatch.SetsInMatch / 2)
        {
            var updateWinnerResult = await _tennisClient.UpdateMatchWinner.ExecuteAsync(new UpdateMatchWinnerInput
            {
                PlayerId = winner.Id,
                MatchId = CurrentMatch.Id
            });
            CurrentMatch.Winner = winner;
            globalStore.OldMatches.Add(CurrentMatch);

        }
    }
    private void SetGameScore(Player winner, Player loser)
    {
        if (CurrentMatch.CurrentSet.CurrentGame.HasWinner)
        {
            CurrentMatch.CurrentSet.CurrentGame.WrittenScore = GetWinningScore(winner, loser);

            if (CurrentMatch.Winner is null)
            {
                CurrentMatch.CurrentSet.CurrentGame.VerbalScore = GetWinningScore(winner, loser, isWrittenScore: false);
            }
            else
            {
                CurrentMatch.CurrentSet.CurrentGame.VerbalScore = $"The winner is {CurrentMatch.Winner.Name}!";
            }
        }
        else if (CurrentMatch.PlayerOne.PointsScored == CurrentMatch.PlayerTwo.PointsScored)
        {
            CurrentMatch.CurrentSet.CurrentGame.WrittenScore = GetScore();
            CurrentMatch.CurrentSet.CurrentGame.VerbalScore = GetDrawScore();
        }
        else
        {
            CurrentMatch.CurrentSet.CurrentGame.WrittenScore = GetScore();
            CurrentMatch.CurrentSet.CurrentGame.VerbalScore = GetScore(isWrittenScore: false);
        }
    }
    private string GetWinningScore(Player winner, Player loser, bool isWrittenScore = true)
    {
        string writtenGamePointForPlayerOne = WrittenGamePoints[CurrentMatch.PlayerOne.PointsScored];
        string writtenGamePointForPlayerTwo = WrittenGamePoints[CurrentMatch.PlayerTwo.PointsScored];

        VerbalGamePoint verbalPointForPlayerOne = (VerbalGamePoint)CurrentMatch.PlayerOne.PointsScored;
        VerbalGamePoint verbalPointForPlayerTwo = (VerbalGamePoint)CurrentMatch.PlayerTwo.PointsScored;

        if (CurrentMatch.PlayerOne.PointsScored > CurrentMatch.PlayerTwo.PointsScored)
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
        if (CurrentMatch.PlayerOne.IsServer)
        {
            return $"{verbalPointForPlayerOne} | {verbalPointForPlayerTwo}";
        }

        return $"{verbalPointForPlayerTwo} | {verbalPointForPlayerOne}";
    }
    private string GetScore(bool isWrittenScore = true)
    {
        if (isWrittenScore)
        {
            return $"{WrittenGamePoints[CurrentMatch.PlayerOne.PointsScored]} | {WrittenGamePoints[CurrentMatch.PlayerTwo.PointsScored]}";
        }

        // Sets right scoring order depending on who's the server.

        if (CurrentMatch.PlayerOne.IsServer)
        {
            return $"{(VerbalGamePoint)CurrentMatch.PlayerOne.PointsScored} | {(VerbalGamePoint)CurrentMatch.PlayerTwo.PointsScored}";
        }
        return $"{(VerbalGamePoint)CurrentMatch.PlayerTwo.PointsScored} | {(VerbalGamePoint)CurrentMatch.PlayerOne.PointsScored}";
    }
    private string GetDrawScore()
    {
        return CurrentMatch.PlayerOne.PointsScored switch
        {
            1 => $"{VerbalGamePoint.Fifteen} - All",
            2 => $"{VerbalGamePoint.Thirty} - All",
            _ => $"{VerbalGamePoint.Deuce}"
        };
    }

    public override string ToString() => Name;
}