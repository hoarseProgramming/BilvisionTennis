
namespace BilvisionTennisAPI.GraphQL.Data
{
    public class Set
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Rally> Rallies { get; set; } = [];
    }
}
