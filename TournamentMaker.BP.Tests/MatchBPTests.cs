using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.BP.Tests
{
    [TestClass]
    public class MatchBPTests
    {
        private readonly Mock<IMatchStore> _matchStoreMock;
        private readonly Mock<IPlayerStore> _playerStoreMock;
        private readonly Mock<ITeamStore> _teamStoreMock;
        private readonly Mock<ISportStore> _sportStoreMock;

        public MatchBPTests()
        {
            _matchStoreMock = new Mock<IMatchStore>(MockBehavior.Strict);
            _playerStoreMock = new Mock<IPlayerStore>(MockBehavior.Strict);
            _teamStoreMock = new Mock<ITeamStore>(MockBehavior.Strict);
            _sportStoreMock = new Mock<ISportStore>(MockBehavior.Strict);
        }

        [TestMethod]
        public async Task Close2PlayersTest()
        {
            var matchBP = new MatchBP(_matchStoreMock.Object, _playerStoreMock.Object, _teamStoreMock.Object, _sportStoreMock.Object);
            var rank1 = new Rank { Level = 1000, SportKey = "sport" };
            var rank2 = new Rank { Level = 1000, SportKey = "sport" };

            var match = new BO.Match
            {
                Id = 1,
                CreatorId = "creator",
                SportKey = "sport",
                Scores = "1-2;3-1;3-2",
                Teams = new List<Team>
                {
                    new Team {Players = new List<Player> {new Player {Matricule = "S123456", Ranks = new Collection<Rank> {rank1}}}, Id = 1},
                    new Team {Players = new List<Player> {new Player {Matricule = "S123457", Ranks = new Collection<Rank> {rank2}}}, Id = 2}
                }
            };

            _matchStoreMock.Setup(m => m.GetWithPlayersAndRanks(1)).ReturnsAsync(match);
            _matchStoreMock.Setup(m => m.SaveChangesAsync()).Returns(() => Task.FromResult(default(Task)));
            var result = await matchBP.Close(match, "creator");

            _matchStoreMock.VerifyAll();
            Assert.AreEqual(match, result);
            Assert.AreEqual(rank1.Level, 1007);
            Assert.AreEqual(rank2.Level, 993);
        }

        [TestMethod]
        public async Task Close2PlayersDrawTest()
        {
            var matchBP = new MatchBP(_matchStoreMock.Object, _playerStoreMock.Object, _teamStoreMock.Object, _sportStoreMock.Object);

            var rank1 = new Rank {Level = 1000, SportKey = "sport"};
            var rank2 = new Rank {Level = 1000, SportKey = "sport"};

            var match = new BO.Match
            {
                Id = 1,
                CreatorId = "creator",
                SportKey = "sport",
                Scores = "1-2;1-1;2-1",
                Teams = new List<Team>
                {
                    new Team {Players = new List<Player> {new Player {Matricule = "S123456", Ranks = new Collection<Rank> {rank1}}}, Id = 1},
                    new Team {Players = new List<Player> {new Player {Matricule = "S123457", Ranks = new Collection<Rank> {rank2}}}, Id = 2}
                }
            };

            _matchStoreMock.Setup(m => m.GetWithPlayersAndRanks(1)).ReturnsAsync(match);
            _matchStoreMock.Setup(m => m.SaveChangesAsync()).Returns(() => Task.FromResult(default(Task)));
            var result = await matchBP.Close(match,"creator");

            _matchStoreMock.VerifyAll();
            Assert.AreEqual(match, result);
            Assert.AreEqual(rank1.Level, 1000);
            Assert.AreEqual(rank2.Level, 1000);
        }

        [TestMethod]
        public async Task Close2PlayersDrawTest2()
        {
            var matchBP = new MatchBP(_matchStoreMock.Object, _playerStoreMock.Object, _teamStoreMock.Object, _sportStoreMock.Object);
            var rank1 = new Rank { Level = 1800, SportKey = "sport", };
            var rank2 = new Rank { Level = 2005, SportKey = "sport" };

            var match = new BO.Match
            {
                Id = 1,
                CreatorId = "creator",
                SportKey = "sport",
                Scores = "1-2;1-1;2-1",
                Teams = new List<Team>
                {
                    new Team {Players = new List<Player> {new Player {Parties = 31, Matricule = "S123456", Ranks = new Collection<Rank> {rank1}}}, Id = 1},
                    new Team {Players = new List<Player> {new Player {Parties = 31, Matricule = "S123457", Ranks = new Collection<Rank> {rank2}}}, Id = 2}
                }
            };

            _matchStoreMock.Setup(m => m.GetWithPlayersAndRanks(1)).ReturnsAsync(match);
            _matchStoreMock.Setup(m => m.SaveChangesAsync()).Returns(() => Task.FromResult(default(Task)));
            var result = await matchBP.Close(match,"creator");

            _matchStoreMock.VerifyAll();
            Assert.AreEqual(match, result);
            Assert.AreEqual(1804, rank1.Level);
            Assert.AreEqual(2001, rank2.Level);
        }
    }
}