using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Umpires
{
    public static class UmpireDataLoaders
    {
        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Umpire>> UmpireByIdAsync(
            IReadOnlyList<int> ids,
            TennisDbContext db,
            ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Umpires
                .AsNoTracking()
                .Where(u => ids.Contains(u.Id))
                .Select(s => s.Id, selector)
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }
    }
}
