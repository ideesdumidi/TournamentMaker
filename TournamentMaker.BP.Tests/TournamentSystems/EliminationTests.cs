using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;
using TournamentMaker.BP.TournamentSystems;

namespace TournamentMaker.BP.Tests.TournamentSystems
{
    [TestClass()]
    public class EliminationTests
    {
        private ICollection<Team> CreateTeams(int nbTeams, int playersByTeam = 2)
        {
            var teams = new List<Team>();
            for (int i = 0; i < nbTeams; i++)
            {
                var team = new Team{Players = new Collection<Player>()};

                for (int j = 0; j < playersByTeam; j++)
                    team.Players.Add(new Player {Matricule = (i*j + j).ToString(CultureInfo.InvariantCulture)});
                
                teams.Add(team);
            }
            return teams;
        }
        private ICollection<Team> CreateTeamsByNbPlayers(int nbPlayers)
        {
            var teams = new List<Team>();
            for (int i = 0; i < nbPlayers * 0.5; i++)
            {
                teams.Add(new Team
                {
                    Players = new Collection<Player>
                {
                    new Player {Matricule = (i*2).ToString(CultureInfo.InvariantCulture)},
                    new Player {Matricule = (i*2+1).ToString(CultureInfo.InvariantCulture)}
                }
                });

            }
            return teams;
        }

        [TestMethod()]
        public void CreateTest5Teams2Teams()
        {
            var eliminationSystem = new Elimination
            {
                Tournament = new EliminationTournament
                {
                    CreatorId = "test",
                    Id = 1,
                    Teams = CreateTeams(5),
                    TeamsByMatch = 2,
                    Sport = new Sport
                    {
                        MinTeams = 2,
                        MaxTeams = 4
                    }
                }
            };
            eliminationSystem.Create();
            Assert.AreEqual(eliminationSystem.Tournament.Qualifications.Count, 3);
        }
        [TestMethod()]
        public void CreateTest8Teams2Teams()
        {
            var eliminationSystem = new Elimination
            {
                Tournament = new EliminationTournament
                {
                    CreatorId = "test",
                    Id = 1,
                    Teams = CreateTeams(8),
                    TeamsByMatch = 2,
                    Sport = new Sport
                    {
                        MinTeams = 2,
                        MaxTeams = 4
                    }
                }
            };
            eliminationSystem.Create();
            Assert.AreEqual(eliminationSystem.Tournament.Qualifications.Count, 3);
        }

        [TestMethod()]
        public void CreateTest9Teams3Teams()
        {
            var eliminationSystem = new Elimination
            {
                Tournament = new EliminationTournament
                {
                    CreatorId = "test",
                    Id = 1,
                    Teams = CreateTeams(9),
                    TeamsByMatch = 3,
                    Sport = new Sport
                    {
                        MinTeams = 2,
                        MaxTeams = 4
                    }
                }
            };
            eliminationSystem.Create();
            Assert.AreEqual(eliminationSystem.Tournament.Qualifications.Count, 2);
        }
        [TestMethod()]
        public void CreateTest5Teams3Teams()
        {
            var eliminationSystem = new Elimination
            {
                Tournament = new EliminationTournament
                {
                    CreatorId = "test",
                    Id = 1,
                    Teams = CreateTeams(9),
                    TeamsByMatch = 3,
                    Sport = new Sport
                    {
                        MinTeams = 2,
                        MaxTeams = 4
                    }
                }
            };
            eliminationSystem.Create();
            Assert.AreEqual(eliminationSystem.Tournament.Qualifications.Count, 2);
        }

        [TestMethod()]
        public void CloseMatchTest()
        {
        }

        [TestMethod()]
        public void CloseTest()
        {
        }
    }
}