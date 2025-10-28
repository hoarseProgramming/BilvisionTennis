using BilvisionTennisAPI.GraphQL.Data;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Umpires
{
    [MutationType]
    public static class UmpireMutations
    {
        public static async Task<Umpire> AddUmpireAsync(AddUmpireInput input, TennisDbContext db, CancellationToken cancellationToken)
        {
            var umpire = new Umpire { Name = input.Name };

            db.Umpires.Add(umpire);

            await db.SaveChangesAsync(cancellationToken);

            return umpire;
        }

    }
}
