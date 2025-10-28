using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Sets
{
    public static class SetDataLoader
    {
        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Set>> SetByIdAsync(
          IReadOnlyList<int> ids,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Sets
                .AsNoTracking()
                .Where(s => ids.Contains(s.Id))
                .Select(s => s.Id, selector)
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }

        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Set[]>> SetsByGameIdAsync(
          IReadOnlyList<int> gameIds,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Games
                .AsNoTracking()
                .Where(g => gameIds.Contains(g.Id))
                .Select(g => g.Id, g => g.Sets, selector)
                .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken);
        }
    }
}
