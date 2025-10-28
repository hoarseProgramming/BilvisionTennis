
namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Player
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Match> Matches { get; set; } = [];
        public List<Match> WonMatches { get; set; } = [];
    }
}
