using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.DAL.Interfaces
{
    public interface ITournamentStore:IStore
    {
        Task<Tournament> Get(int id);
        Task<ICollection<Tournament>> Get();
        Task<Tournament> Add(Tournament tournament);
        Task<Tournament> SetState(Tournament tournament, EntityState entityState);
    }
}