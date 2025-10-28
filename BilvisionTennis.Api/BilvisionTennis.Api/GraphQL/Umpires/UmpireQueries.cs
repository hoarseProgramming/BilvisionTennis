using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Umpires
{
    [QueryType]
    public static class UmpireQueries
    {
        public static async Task<IEnumerable<Umpire>> GetUmpiresAsync(TennisDbContext db, CancellationToken cancellationToken)
        {
            return await db.Umpires.AsNoTracking().ToListAsync(cancellationToken);
        }

        public static async Task<Umpire?> GetUmpireAsync(
            int id,
            IUmpireByIdDataLoader umpireById,
            ISelection selection,
            CancellationToken cancellationToken)
        {
            return await umpireById.Select(selection).LoadAsync(id, cancellationToken);
        }
    }
}
