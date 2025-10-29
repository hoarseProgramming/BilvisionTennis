using BilvisionTennis.Frontend.GraphQl;
using BilvisionTennis.Frontend.TennisGame;

namespace BilvisionTennis
{
    public class GlobalStore(ITennisClient tennisClient)
    {

        public Player PlayerOne { get; set; } = new() { Name = "PlayerOne" };
        public Player PlayerTwo { get; set; } = new() { Name = "PlayerTwo" };
        public Umpire? Umpire { get; set; }
        public TennisMatch? CurrentMatch { get; set; }
        public int NumberOfSetsInGame { get; set; }
        public List<TennisMatch> OldMatches { get; set; } = [];
    }
}
