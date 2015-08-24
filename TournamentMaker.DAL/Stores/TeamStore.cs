using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.DAL.Stores
{
    public class TeamStore : ITeamStore
    {
        private readonly MatchContext _matchContext;

        public TeamStore(MatchContext matchContext)
        {
            _matchContext = matchContext;
        }

        public async Task<ICollection<Team>> Add(ICollection<Team> teams)
        {
            _matchContext.Teams.AddRange(teams);

            return await Task.FromResult(teams);
        }

        public Task<Team> SetState(Team team, EntityState entityState)
        {
            _matchContext.Entry(team).State = entityState;
            return Task.FromResult(team);
        }

        public async Task SaveChangesAsync()
        {
            await _matchContext.SaveChangesAsync();
        }
    }
}
