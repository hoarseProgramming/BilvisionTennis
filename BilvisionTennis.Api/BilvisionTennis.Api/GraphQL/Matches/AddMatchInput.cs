namespace BilvisionTennisAPI.GraphQL.GraphQL.Matches
{
    public sealed record AddMatchInput(List<int> players, int umpireId);
}
