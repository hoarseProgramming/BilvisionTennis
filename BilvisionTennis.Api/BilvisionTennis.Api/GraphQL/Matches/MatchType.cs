using BilvisionTennisAPI.GraphQL.Data;
using BilvisionTennisAPI.GraphQL.GraphQL.Games;
using BilvisionTennisAPI.GraphQL.GraphQL.Players;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Types
{
    [ObjectType<Match>]
    public static partial class MatchType
    {
        public static async Task<IEnumerable<Player>> GetPlayersAsync(
            [Parent] Match match,
            IPlayersByMatchIdDataLoader playersByMatchId,
            ISelection selection,
             CancellationToken cancellationToken)
        {
            return await playersByMatchId
                .Select(selection)
                .LoadRequiredAsync(match.Id, cancellationToken);
        }

        public static async Task<IEnumerable<Game>> GetGamesAsync(
            [Parent] Match match,
            IGamesByMatchIdDataLoader gamesByMatchId,
            ISelection selection,
             CancellationToken cancellationToken)
        {
            return await gamesByMatchId
                .Select(selection)
                .LoadRequiredAsync(match.Id, cancellationToken);
        }
    }
}
