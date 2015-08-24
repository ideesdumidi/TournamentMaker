using System;
using System.Collections.Generic;
using System.Linq;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public class PoolElimination : TournamentSystem<PoolEliminationTournament>
    {
        public override void Create()
        {
            var nbTeams = (int)(Tournament.Teams.SelectMany(t => t.Players).Count(p => p != null) * 0.5);

            Qualification nextQualification = null;
            var level = 0;
            int levelTeamMax;

            //Création des qualifs
            do
            {
                level++;
                levelTeamMax = (int)Math.Pow(2, level);
                string name = "Qualifications";
                switch (level)
                {
                    case 1:
                        name = "Finale";
                        break;
                    case 2:
                        name = "Demi-finale";
                        break;
                    case 3:
                        name = "Quart de finale";
                        break;
                    case 4:
                        name = "Huitième de finale";
                        break;
                    case 5:
                        name = "Seizième de finale";
                        break;
                }
                var qualification = new Qualification { Tournament = Tournament, NextQualification = nextQualification, Name = name, Matchs = new List<Match>() };
                if (nextQualification != null) nextQualification.PreviousQualifications.Add(qualification);
                //TODO:Creation des matchs

                nextQualification = qualification;

                Tournament.Qualifications.Add(nextQualification);

            } while (nbTeams > levelTeamMax && ((int)Math.Pow(2, level + 1) == nbTeams || nbTeams < levelTeamMax * 3 || nbTeams >= levelTeamMax * 6));

            //Création des pools
            if (nbTeams >= levelTeamMax * 3 && nbTeams < levelTeamMax * 6)
            {
                for (int i = 0; i < levelTeamMax; i++)
                {
                    var pool = new BO.Pool { Tournament = Tournament, NextQualification = nextQualification };
                    nextQualification.PreviousQualifications.Add(pool);
                    if (Tournament.PoolFreeForAll)
                        pool.Matchs = new List<Match> { new Match() };
                    else
                    {
                        //TODO: création des matchs
                    }
                Tournament.Qualifications.Add(pool);
                }
            }
        }

        public override void CloseMatch(Match match)
        {
            
        }

        public override void Close()
        {
            
        }
    }
}
