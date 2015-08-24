using System.Collections.Generic;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BO
{
    public class Team
    {
        public int Id { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}