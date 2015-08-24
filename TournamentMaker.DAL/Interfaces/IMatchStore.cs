using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TournamentMaker.BO;

namespace TournamentMaker.DAL.Interfaces
{
    public interface IMatchStore : IStore
    {
        Task<Match> Get(int id);
        Task<ICollection<Match>> Get();
        Task<Match> Add(Match match);
        Task<Match> SetState(Match match, EntityState entityState);

        Task<Match> GetWithPlayersAndRanks(int id);
    }
}