using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentMaker.BO
{
    public class Match
    {
        public Match()
        {
            Teams = new HashSet<Team>();
        }
        public int Id { get; set; }

        public int? WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public Team Winner { get; set; }
        public ICollection<Team> Teams { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public Player Creator { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Name { get; set; }
        public int Sleeves { get; set; }
        public int Points { get; set; }
        public bool Ranked { get; set; }
        public bool Private { get; set; }
        public bool Randomize { get; set; }
        public string Scores { get; set; }

        public string SportKey { get; set; }
        [ForeignKey("SportKey")]
        public Sport Sport { get; set; }

        public StateEnum State { get; set; }

        public Qualification Qualification { get; set; }
        public int? QualificationId { get; set; }
    }

    public enum StateEnum
    {
        Opened,
        InProgress,
        Won,
        Draw
    }
}