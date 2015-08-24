using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BO
{
    public class Qualification
    {
        public Qualification()
        {
            PreviousQualifications = new Collection<Qualification>();
            Matchs = new Collection<Match>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Match> Matchs { get; set; }
        public Qualification NextQualification { get; set; }
        public ICollection<Qualification> PreviousQualifications { get; set; }
        [ForeignKey("TournamentId")]
        public Tournament Tournament { get; set; }
        public int TournamentId { get; set; }
        public int NbTeams { get; set; }
        public int Sleeves { get; set; }
        public bool Active { get; set; }
    }
}