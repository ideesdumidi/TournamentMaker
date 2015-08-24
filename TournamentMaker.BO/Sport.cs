using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentMaker.BO
{
    public class Sport
    {
        [Key]
        public string Key { get; set; }
        public String Name { get; set; }
        public int MaxPlayers { get; set; }
        public int MinTeams { get; set; }
        public int MaxTeams { get; set; }
        public virtual ICollection<Rank> Ranks { get; set; }
    }
}