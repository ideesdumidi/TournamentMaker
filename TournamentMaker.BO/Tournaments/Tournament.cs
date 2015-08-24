using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentMaker.BO.Tournaments
{
    public class Tournament
    {
        public Tournament()
        {
            Teams = new Collection<Team>();
            Qualifications = new Collection<Qualification>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public Player Creator { get; set; }
        public int? WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public Team Winner { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Qualification> Qualifications { get; set; }
        public string Name { get; set; }
        public bool Private { get; set; }
        public bool Ranked { get; set; }
        public bool Randomize { get; set; }

        public string SportKey { get; set; }
        [ForeignKey("SportKey")]
        public Sport Sport { get; set; }
        public StateEnum State { get; set; }
        public string Type { get; set; }
    }
}