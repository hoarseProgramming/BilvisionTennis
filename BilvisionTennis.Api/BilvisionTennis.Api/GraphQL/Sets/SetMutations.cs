using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Sets
{
    [MutationType]
    public static class SetMutations
    {
        public static async Task<Set> AddSetAsync(
            AddSetInput input,
            TennisDbContext db,
            CancellationToken cancellationToken
            )
        {
            var game = await db.Games.FirstOrDefaultAsync(g => g.Id == input.gameId);

            if (game == null) throw new GraphQLException("Game not found");
            var set = new Set { Number = input.Number };

            game.Sets.Add(set);

            await db.SaveChangesAsync();

            return set;
        }
    }
}
