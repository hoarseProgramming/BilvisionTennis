using BilvisionTennisAPI.GraphQL.Data;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Players
{
    [MutationType]
    public static class PlayerMutations
    {
        public static async Task<Player> AddPlayerAsync(AddPlayerInput input, TennisDbContext db, CancellationToken cancellationToken)
        {
            var player = new Player { Name = input.Name };

            db.Players.Add(player);

            await db.SaveChangesAsync(cancellationToken);

            return player;
        }
    }
}
