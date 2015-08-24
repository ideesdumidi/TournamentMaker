using System.Collections.Generic;
using System.Threading.Tasks;
using TournamentMaker.BO;

namespace TournamentMaker.DAL.Interfaces
{
    public interface ISportStore:IStore
    {
        Task<ICollection<Sport>> Get();

        Task<Sport> Get(string key);
    }
}