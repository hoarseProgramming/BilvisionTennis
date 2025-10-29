using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Players
{
    [QueryType]
    public static class PlayerQueries
    {
        public static async Task<IEnumerable<Player>> GetPlayersAsync(TennisDbContext db, CancellationToken cancellationToken)
        {
            return await db.Players.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
