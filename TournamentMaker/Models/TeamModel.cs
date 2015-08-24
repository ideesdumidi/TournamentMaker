using System.Collections.Generic;
using System.Linq;
using TournamentMaker.BO;

namespace TournamentMaker.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public virtual ICollection<PlayerModel> Players { get; set; }
        public string Name { get; set; }

        public static TeamModel From(Team team)
        {
            if (team == null)
                return null;
            return new TeamModel
            {
                Id = team.Id,
                Name = team.Name,
                Players = team.Players.Select(PlayerModel.From).ToList()
            };
        }
    }
}