using System.Collections.Generic;
using System.Linq;
using TournamentMaker.BO;

namespace TournamentMaker.Models
{
    public class QualificationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MatchModel> Matchs { get; set; }
        public virtual ICollection<QualificationModel> PreviousQualifications { get; set; }
        public int NextQualificationId { get; set; }
        public int NbTeams { get; set; }
        public int Sleeves { get; set; }
        public bool Active { get; set; }

        public static QualificationModel From(Qualification qualification)
        {
            if (qualification == null)
                return null;
            var result = new QualificationModel
            {
                Id = qualification.Id,
                Name = qualification.Name,
                Matchs = qualification.Matchs.Select(MatchModel.From).ToList(),
                PreviousQualifications = qualification.PreviousQualifications.Select(From).ToList(),
                NbTeams = qualification.NbTeams,
                Active = qualification.Active,
                Sleeves = qualification.Sleeves
            };

            if (qualification.NextQualification != null)
                result.NextQualificationId = qualification.NextQualification.Id;

            return result;

        }
    }
}