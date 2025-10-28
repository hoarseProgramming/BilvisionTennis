using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Rallies
{
    [MutationType]
    public static class RallyMutations
    {
        public static async Task<Rally> AddRallyAsync(
           AddRallyInput input,
           TennisDbContext db,
           CancellationToken cancellationToken
           )
        {
            var set = await db.Sets.FirstOrDefaultAsync(s => s.Id == input.SetId);

            if (set == null) throw new GraphQLException("Set not found");

            var player = await db.Players.SingleOrDefaultAsync(p => p.Id == input.ScorerId);

            if (player == null) throw new GraphQLException("Player not found");


            var rally = new Rally { ShotNumber = input.Number, Scorer = player };

            set.Rallies.Add(rally);

            await db.SaveChangesAsync();

            return rally;
        }
    }
}
