
namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Rally
    {
        public int Id { get; set; }
        public int ShotNumber { get; set; }
        public required Player Scorer { get; set; }
    }
}
