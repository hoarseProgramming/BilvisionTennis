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
    }
}
