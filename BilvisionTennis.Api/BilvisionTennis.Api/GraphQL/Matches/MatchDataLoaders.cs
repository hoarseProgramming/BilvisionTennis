using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Matches
{
    public static class MatchDataLoaders
    {
        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Match>> MatchByIdAsync(
       IReadOnlyList<int> ids,
       TennisDbContext db,
       ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Matches
                .AsNoTracking()
                .Where(m => ids.Contains(m.Id))
                .Select(s => s.Id, selector)
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }

        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Match[]>> MatchesByUmpireIdAsync(
          IReadOnlyList<int> umpireIds,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Umpires
                .AsNoTracking()
                .Where(m => umpireIds.Contains(m.Id))
                .Select(u => u.Id, u => u.Matches, selector)
                .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken);
        }
    }
}
