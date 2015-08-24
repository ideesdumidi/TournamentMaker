using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public class Pool : TournamentSystem<PoolTournament>
    {
        public override void Create()
        {
            var teams = Tournament.Teams.Where(t => t.Players.Count > 0).ToList();
            var mod = 0;
            var sizePool = 0;
            //Selection de la taille de pool adéquat
            for (int i = (Tournament.FreeForAll ? Tournament.Sport.MinTeams : 4); i < (Tournament.FreeForAll ? Tournament.Sport.MaxTeams : 6); i++)
            {
                var pools = teams.Count/i;
                //Si il y a plus de 1 poule, il faut qu'il y en ai assez pour pouvoir faire un second tour
                if (sizePool != 0 && mod >= teams.Count%i && pools != 1 && (pools<Tournament.Sport.MinTeams || pools>Tournament.Sport.MaxTeams)) continue;
                sizePool = i;
                mod = teams.Count%i;
            }
            //Création des matchs
            while (teams.Count > 0)
            {
                    var qualification = new Qualification {Tournament = Tournament};
                if (Tournament.FreeForAll)
                {
                    var newMatch = new Match();
                    qualification.Matchs.Add(newMatch);
                    while (teams.Count > 0 && newMatch.Teams.Count < sizePool)
                    {
                        newMatch.Teams.Add(teams.First());
                        teams.RemoveAt(0);
                    }
                }
                else
                {
                    while (teams.Count > 0)
                    {
                        List<Team> teamsPool = teams.GetRange(0, sizePool);
                        teams.RemoveRange(0,sizePool);
                        while (teamsPool.Count > 0)
                        {
                            for (int i = 1; i < teamsPool.Count; i++)
                            {
                            var newMatch = new Match();
                            newMatch.Teams.Add(teamsPool.First());
                            newMatch.Teams.Add(teamsPool.ElementAt(i));
                            }
                            teams.RemoveAt(0);
                        }
                    }
                }
            }
        }


        public override void CloseMatch (Match match)
        {
        }

        public override void Close()
        {
        }
    }
}