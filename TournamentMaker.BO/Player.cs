using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentMaker.BO
{
    public class Player
    {
        [Key]
        public string Matricule { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
        public int Parties { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}