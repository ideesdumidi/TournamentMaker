using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;
using TournamentMaker.BP.TournamentSystems;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.BP.Tests
{
    [TestClass()]
    public class TournamentBPTests
    {
        private readonly Mock<ITeamStore> _teamStoreMock;
        private readonly Mock<ITournamentStore> _tournamentStoreMock;
        private readonly Mock<IPlayerStore> _playerStoreMock;
        private readonly Mock<ISportStore> _sportStoreMock;
        private readonly Mock<ITournamentSystem<Tournament>> _tournamentSystemMock;
        private readonly Mock<ITournamentSystemFactory> _tournamentSystemFactoryMock;

        public TournamentBPTests()
        {
            _teamStoreMock = new Mock<ITeamStore>(MockBehavior.Strict);
            _tournamentStoreMock = new Mock<ITournamentStore>(MockBehavior.Strict);
            _playerStoreMock = new Mock<IPlayerStore>(MockBehavior.Strict);
            _sportStoreMock = new Mock<ISportStore>(MockBehavior.Strict);
            _tournamentSystemMock = new Mock<ITournamentSystem<Tournament>>();
            _tournamentSystemFactoryMock = new Mock<ITournamentSystemFactory>();
            
        }

        private ICollection<Team> CreateTeams(int nbPlayers)
        {
            var teams = new List<Team>();
            for (int i = 0; i < nbPlayers*0.5; i++)
            {
                teams.Add(new Team {Players = new Collection<Player>
                {
                    new Player {Matricule = (i*2).ToString(CultureInfo.InvariantCulture)},
                    new Player {Matricule = (i*2+1).ToString(CultureInfo.InvariantCulture)}
                }});

            }
            return teams;
        }

        [TestMethod()]
        public async Task CreateTournamentFinaleTest()
        {
            var matchBP = new TournamentBP(_playerStoreMock.Object, _teamStoreMock.Object,_sportStoreMock.Object, _tournamentStoreMock.Object, _tournamentSystemFactoryMock.Object);
            var tournament = new Tournament
            {
                CreatorId = "test",
                Id = 1,
                Teams = CreateTeams(4)
            };

            _tournamentStoreMock.Setup(t => t.Get(1)).ReturnsAsync(tournament);
            _tournamentSystemFactoryMock.Setup(m => m.Get(tournament)).Returns(_tournamentSystemMock.Object);
            _tournamentSystemMock.Setup(m => m.Create());
            _tournamentStoreMock.Setup(m => m.SaveChangesAsync()).Returns(() => Task.FromResult(default(Task)));

            await matchBP.CreateMatchs(new Tournament { Id = 1 }, "test");
            _tournamentSystemMock.Verify(m=>m.Create(),Times.Once());

            Assert.IsNotNull(tournament.Qualifications);
        }

    }
}
