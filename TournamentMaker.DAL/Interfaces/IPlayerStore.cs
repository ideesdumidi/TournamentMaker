using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentMaker.BO;

namespace TournamentMaker.DAL.Interfaces
{
    public interface IPlayerStore : IStore
    {
        Task<ICollection<Player>> GetFromMatricules(ICollection<string> matricules);
        Task<Player> GetFromMatricule(string matricule);

        Task<ICollection<Player>> Get();
        Task<Player> Add(Player player);

        Task<Player> SetState(Player p, System.Data.Entity.EntityState entityState);
    }
}