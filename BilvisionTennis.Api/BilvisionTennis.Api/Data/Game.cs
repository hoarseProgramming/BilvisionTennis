namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Game
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Set> Sets { get; set; } = [];

    }
}
