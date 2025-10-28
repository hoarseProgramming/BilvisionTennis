using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Rallies
{
    public class RallyDataLoaders
    {
        //[DataLoader]
        //public static async Task<IReadOnlyDictionary<int, Rally>> GameByIdAsync(
        //  IReadOnlyList<int> ids,
        //  TennisDbContext db,
        //  ISelectorBuilder selector, CancellationToken cancellationToken)
        //{
        //    return await db.Games
        //        .AsNoTracking()
        //        .Where(g => ids.Contains(g.Id))
        //        .Select(s => s.Id, selector)
        //        .ToDictionaryAsync(s => s.Id, cancellationToken);
        //}

        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Rally[]>> RalliesBySetIdAsync(
          IReadOnlyList<int> setIds,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Sets
                .AsNoTracking()
                .Where(s => setIds.Contains(s.Id))
                .Select(s => s.Id, m => m.Rallies, selector)
                .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken);
        }
    }
}
