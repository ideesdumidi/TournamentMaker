using System;
using Microsoft.Practices.Unity;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.BP.TournamentSystems
{
    public class TournamentSystemFactory : ITournamentSystemFactory
    {

        private readonly IUnityContainer _container;
        public TournamentSystemFactory(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public ITournamentSystem Get(Tournament tournament)
        {
            var type = tournament.GetType();
            return (ITournamentSystem)_container.Resolve(typeof(ITournamentSystem<>).MakeGenericType(type));
        }
    }
}