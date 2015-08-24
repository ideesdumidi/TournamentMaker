using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public interface ITournamentSystemFactory
    {
        ITournamentSystem Get(Tournament tournament);
    }
}