using BilvisionTennisAPI.GraphQL.Data;
using BilvisionTennisAPI.GraphQL.GraphQL.Rallies;
using GreenDonut.Data;
using HotChocolate.Execution.Processing;

namespace BilvisionTennisAPI.GraphQL.GraphQL.Sets
{
    [ObjectType<Set>]
    public static partial class SetType
    {
        public static async Task<IEnumerable<Rally>> GetRalliesAsync(
    [Parent] Set set,
    IRalliesBySetIdDataLoader ralliesBySetId,
    ISelection selection,
     CancellationToken cancellationToken)
        {
            return await ralliesBySetId
                .Select(selection)
                .LoadRequiredAsync(set.Id, cancellationToken);
        }
    }
}
