using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public abstract class TournamentSystem<T> : ITournamentSystem<T> where T : Tournament
    {
        public T Tournament { get; set; }

        Tournament ITournamentSystem.Tournament
        {
            get { return Tournament; }
            set { Tournament = (T)value; }
        }

        public abstract void Create();
        public abstract void CloseMatch(Match match);
        public abstract void Close();
    }
}
