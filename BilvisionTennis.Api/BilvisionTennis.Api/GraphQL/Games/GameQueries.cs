using BilvisionTennisAPI.GraphQL.Data;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;


namespace BilvisionTennisAPI.GraphQL.GraphQL.Games
{
    [QueryType]
    public static class GameQueries
    {
        public static async Task<Game?> GetGameAsync(
          int id,
          IGameByIdDataLoader gameById,
          ISelection selection,
          CancellationToken cancellationToken)
        {
            return await gameById.Select(selection).LoadAsync(id, cancellationToken);
        }
    }
}
