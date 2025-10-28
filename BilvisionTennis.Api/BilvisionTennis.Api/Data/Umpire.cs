namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Umpire
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Match> Matches { get; set; } = [];
    }
}
