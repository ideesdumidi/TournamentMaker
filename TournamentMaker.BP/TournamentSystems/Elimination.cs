using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public class Elimination : TournamentSystem<EliminationTournament>
    {
        public override void Create()
        {
            if (Tournament.TeamsByMatch < Tournament.Sport.MinTeams || Tournament.TeamsByMatch > Tournament.Sport.MaxTeams)
                throw new InvalidDataException("Le nombre d'équipes par match est incorrect");

            var teams = Tournament.Teams.Where(t => t.Players.Count > 0).ToList();

            Qualification nextQualification = null;

            var levelMax = 0;
            int level;
            //Création des qualifs
            //Calcul (crade) du level max
            while (teams.Count > (int)Math.Pow(Tournament.TeamsByMatch, levelMax))
            {
                levelMax++;
            } 

            for (level = 0; level < levelMax; level++)
            {

                string name = "Qualifications";
                switch (level)
                {
                    case 0:
                        name = "Finale";
                        break;
                    case 1:
                        name = "Demi-finale";
                        break;
                    case 2:
                        name = "Quart de finale";
                        break;
                    case 3:
                        name = "Huitième de finale";
                        break;
                    case 4:
                        name = "Seizième de finale";
                        break;
                }
                var qualification = new Qualification
                {
                    Tournament = Tournament,
                    NextQualification = nextQualification,
                    Name = name, Matchs = new List<Match>(),
                    NbTeams = Tournament.TeamsByMatch,
                    Sleeves = Tournament.Sleeves
                };
                if (nextQualification != null) nextQualification.PreviousQualifications.Add(qualification);

                nextQualification = qualification;

                //Création des matchs
                for (int i = 0; i < (int) Math.Pow(Tournament.TeamsByMatch, level); i++)
                {
                    if(i>teams.Count/Tournament.TeamsByMatch)
                        break;

                    var newMatch = new Match
                    {
                        CreatorId = Tournament.CreatorId,
                        Date = Tournament.Date + TimeSpan.FromDays(levelMax -level),
                        Teams = new List<Team>(),
                        SportKey = Tournament.SportKey,
                        Private = Tournament.Private,
                        Sleeves = Tournament.Sleeves,
                        Ranked = Tournament.Ranked,
                    };
                    nextQualification.Matchs.Add(newMatch);
                }

                Tournament.Qualifications.Add(nextQualification);
            }

            //On assigne les équipes au dernier niveau
            if(nextQualification != null)
            for (int i = 0; i < nextQualification.Matchs.Count; i++)
            {
                var newMatch = nextQualification.Matchs.ElementAt(i);
                while (teams.Count > 0 && newMatch.Teams.Count < Tournament.TeamsByMatch)
                {
                    newMatch.Teams.Add(teams.First());
                    teams.RemoveAt(0);
                }
            }
        }

        public override void CloseMatch(Match match)
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}