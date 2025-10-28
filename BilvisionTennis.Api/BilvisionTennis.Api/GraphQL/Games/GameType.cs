using BilvisionTennisAPI.GraphQL.Data;
using BilvisionTennisAPI.GraphQL.GraphQL.Sets;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Games
{
    [ObjectType<Game>]
    public static partial class GameType
    {
        public static async Task<IEnumerable<Set>> GetSetsAsync(
            [Parent] Game game,
            ISetsByGameIdDataLoader setsByGameId,
            ISelection selection,
            CancellationToken cancellationToken)
        {
            return await setsByGameId
                .Select(selection)
                .LoadRequiredAsync(game.Id, cancellationToken);
        }
    }
}
