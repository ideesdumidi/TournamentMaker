using System.Collections.Generic;

namespace TournamentMaker.Models
{
    public class PoolModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Order { get; set; }
        public virtual ICollection<MatchModel> Matchs { get; set; }
    }
}