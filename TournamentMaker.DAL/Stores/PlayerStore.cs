using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.DAL.Interfaces;

namespace TournamentMaker.DAL.Stores
{
    public class PlayerStore : IPlayerStore
    {
        private readonly MatchContext _matchContext;

        public PlayerStore(MatchContext matchContext)
        {
            if (matchContext == null) throw new ArgumentNullException("matchContext");
            _matchContext = matchContext;
        }

        public async Task<ICollection<Player>> GetFromMatricules(ICollection<string> matricules)
        {
            return await _matchContext.Players.Where(j => matricules.Contains(j.Matricule)).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _matchContext.SaveChangesAsync();
        }

        public async Task<Player> GetFromMatricule(string matricule)
        {
            return await _matchContext.Players.FirstOrDefaultAsync(p => p.Matricule == matricule);
        }

        public async Task<ICollection<Player>> Get()
        {
            return await _matchContext.Players.ToListAsync();

        }

        public async Task<Player> Add(Player player)
        {
            _matchContext.Players.Add(player);
            return await Task.FromResult(player);
        }

        public async Task<Player> SetState(Player player, EntityState entityState)
        {
            _matchContext.Entry(player).State = entityState;
            return await Task.FromResult(player);
        }
    }
}
