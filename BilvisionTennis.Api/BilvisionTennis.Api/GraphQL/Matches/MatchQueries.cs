using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;


namespace BilvisionTennisAPI.GraphQL.GraphQL.Matches
{
    [QueryType]
    public static class MatchQueries
    {

        public static async Task<Match?> GetMatchAsync(
          int id,
          IMatchByIdDataLoader matchById,
          ISelection selection,
          CancellationToken cancellationToken)
        {
            return await matchById.Select(selection).LoadAsync(id, cancellationToken);
        }

    }
}
