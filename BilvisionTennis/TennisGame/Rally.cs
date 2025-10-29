namespace BilvisionTennis.Frontend.TennisGame
{
    public class Rally
    {
        public int Id { get; set; }
        public required Player Winner { get; set; }
        public int Number { get; set; }
    }
}
