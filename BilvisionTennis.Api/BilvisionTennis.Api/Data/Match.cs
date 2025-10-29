
namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Match
    {
        public int Id { get; set; }
        public int? WInnerId { get; set; }
        public Player? Winner { get; set; }
        public List<Game> Games { get; set; } = [];
        public List<Player> Players { get; set; } = [];
        public int UmpireId { get; set; }
        public required Umpire Umpire { get; set; }
    }
}
