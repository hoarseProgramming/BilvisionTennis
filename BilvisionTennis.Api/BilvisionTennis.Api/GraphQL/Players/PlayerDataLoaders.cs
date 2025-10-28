using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;


namespace BilvisionTennisAPI.GraphQL.GraphQL.Players
{
    public class PlayerDataLoaders
    {
        [DataLoader]
        public static async Task<IReadOnlyDictionary<int, Player[]>> PlayersByMatchIdAsync(
      IReadOnlyList<int> matchIds,
      TennisDbContext db,
      ISelectorBuilder selector, CancellationToken cancellationToken)
        {
            return await db.Matches
                .AsNoTracking()
                .Where(m => matchIds.Contains(m.Id))
                .Select(s => s.Id, s => s.Players, selector)
                .ToDictionaryAsync(r => r.Key, r => r.Value.ToArray(), cancellationToken);
        }
    }

}
