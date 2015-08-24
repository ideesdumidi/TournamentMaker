using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TournamentMaker.BO;

namespace TournamentMaker.Models
{
    public class MatchModel
    {
        public int Id { get; set; }

        public int? WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public TeamModel Winner { get; set; }
        public virtual ICollection<TeamModel> Teams { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public PlayerModel Creator { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Name { get; set; }
        public int Sleeves { get; set; }
        public int Points { get; set; }
        public bool Ranked { get; set; }
        public bool Private { get; set; }
        public bool Randomize { get; set; }
        public string SportKey { get; set; }
        public StateEnum State { get; set; }
        public string Scores { get; set; }

        public static MatchModel From(Match match)
        {
            if (match == null)
                return null;
            return new MatchModel
            {
                CreatorId = match.CreatorId,
                Creator = PlayerModel.From(match.Creator),
                Date = match.Date,
                CloseDate = match.CloseDate,
                Id = match.Id,
                Name = match.Name,
                Points = match.Points,
                Private = match.Private,
                Ranked = match.Ranked,
                Randomize = match.Randomize,
                SportKey = match.SportKey,
                WinnerId = match.WinnerId,
                Sleeves = match.Sleeves,
                Teams = match.Teams.Select(TeamModel.From).ToList(),
                State = match.State,
                Scores = match.Scores,
            };
        }

    }

}