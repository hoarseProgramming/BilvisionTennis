using BilvisionTennisAPI.GraphQL.Data;
using BilvisionTennisAPI.GraphQL.GraphQL.Matches;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Umpires
{

    [ObjectType<Umpire>]
    public static partial class UmpireType
    {
        public static async Task<IEnumerable<Match>> GetMatchesAsync(
            [Parent] Umpire umpire,
            IMatchesByUmpireIdDataLoader matchesByUmpireId,
            ISelection selection,
                CancellationToken cancellationToken)
        {
            return await matchesByUmpireId
                .Select(selection)
                .LoadRequiredAsync(umpire.Id, cancellationToken);
        }
    }

}
