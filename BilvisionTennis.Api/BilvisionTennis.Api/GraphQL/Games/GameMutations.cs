using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Games
{
    [MutationType]
    public static class GameMutations
    {
        public static async Task<Game> AddGameAsync(AddGameInput input, TennisDbContext db, CancellationToken cancellationToken)
        {
            var match = await db.Matches.SingleOrDefaultAsync(m => m.Id == input.MatchId);

            if (match == null) throw new GraphQLException("The match wasn't found");

            var game = new Game { Number = input.Number, Sets = new List<Set> { new Set { Number = 0 } } };

            match.Games.Add(game);

            await db.SaveChangesAsync(cancellationToken);

            return game;
        }
    }
}
