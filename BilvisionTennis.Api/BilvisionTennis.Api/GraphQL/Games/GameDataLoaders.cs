using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;


namespace BilvisionTennisAPI.GraphQL.GraphQL.Games
{
    public static class GameDataLoaders
    {
        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Game>> GameByIdAsync(
          IReadOnlyList<int> ids,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Games
                .AsNoTracking()
                .Where(g => ids.Contains(g.Id))
                .Select(s => s.Id, selector)
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }

        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Game[]>> GamesByMatchIdAsync(
          IReadOnlyList<int> matchIds,
          TennisDbContext db,
          ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Matches
                .AsNoTracking()
                .Where(m => matchIds.Contains(m.Id))
                .Select(m => m.Id, m => m.Games, selector)
                .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken);
        }
    }
}
