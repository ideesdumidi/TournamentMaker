using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TournamentMaker.BO;

namespace TournamentMaker.DAL.Interfaces
{
    public interface ITeamStore:IStore
    {
        Task<ICollection<Team>> Add(ICollection<Team> teams);
        Task<Team> SetState(Team team, EntityState entityState);
    }
}