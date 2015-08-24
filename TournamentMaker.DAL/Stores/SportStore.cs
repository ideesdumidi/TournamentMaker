using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.DAL.Stores
{
    public class SportStore : ISportStore
    {
        private readonly MatchContext _matchContext;

        public SportStore(MatchContext matchContext)
        {
            _matchContext = matchContext;
        }

        public async Task<ICollection<Sport>> Get()
        {

            return await _matchContext.Sports.ToListAsync();
        }

        public async Task<Sport> Get(string key)
        {

            return await _matchContext.Sports.FirstOrDefaultAsync(s=>s.Key == key);
        }

        public async Task SaveChangesAsync()
        {
            await _matchContext.SaveChangesAsync();
        }
    }
}
