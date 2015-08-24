using System.Threading.Tasks;

namespace TournamentMaker.DAL.Interfaces
{
    public interface IStore
    {
        Task SaveChangesAsync();
    }
}