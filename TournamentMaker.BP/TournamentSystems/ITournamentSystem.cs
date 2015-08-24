using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public interface ITournamentSystem
    {
        Tournament Tournament { get; set; }
        void Create();
        void CloseMatch(Match match);
        void Close();
    }

    public interface ITournamentSystem<T>:ITournamentSystem where T:Tournament
    {
         new T Tournament { get; set; }
    }
}