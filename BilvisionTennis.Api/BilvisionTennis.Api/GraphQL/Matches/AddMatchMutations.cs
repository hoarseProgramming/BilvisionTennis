using BilvisionTennisAPI.GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Matches
{
    [MutationType]
    public static class AddMatchMutations
    {
        public static async Task<Match> AddMatchAsync(AddMatchInput input, TennisDbContext db, CancellationToken cancellationToken)
        {
            var players = await db.Players.Where(p => input.players.Contains(p.Id)).ToListAsync(cancellationToken);

            if (players.Count != input.players.Count) throw new GraphQLException("Players not found");

            var umpire = await db.Umpires.FirstOrDefaultAsync(u => u.Id == input.umpireId);

            if (umpire == null) throw new GraphQLException("Umpire not found");

            var match = new Match { Players = players, Umpire = umpire };

            var game = new Game { Number = 0, Sets = new List<Set> { new Set { Number = 0 } } };

            match.Games.Add(game);

            db.Matches.Add(match);

            await db.SaveChangesAsync(cancellationToken);

            return match;
        }

    }
}
