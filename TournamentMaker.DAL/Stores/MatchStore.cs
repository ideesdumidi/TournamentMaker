using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.DAL.Stores
{
    public class MatchStore : IMatchStore
    {
        private readonly MatchContext _matchContext;

        public MatchStore(MatchContext matchContext)
        {
            _matchContext = matchContext;
        }

        public async Task<Match> Get(int id)
        {
            return await _matchContext.Matches.Include("Teams").Include("Teams.Players").FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<ICollection<Match>> Get()
        {
            return await _matchContext.Matches.Include("Teams").Include("Teams.Players").OrderBy(e => e.Date).Take(10).ToListAsync();
        }

        public async Task<Match> Add(Match match)
        {
            _matchContext.Matches.Add(match);
            return await Task.FromResult(match);
        }

        public async Task<Match> SetState(Match match, EntityState entityState)
        {
            _matchContext.Entry(match).State = entityState;
            return await Task.FromResult(match);
        }

        public async Task<Match> GetWithPlayersAndRanks(int id)
        {
            return await _matchContext.Matches.Include("Teams").Include("Teams.Players").Include("Teams.Players.Ranks").FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _matchContext.SaveChangesAsync();
        }
    }
}
