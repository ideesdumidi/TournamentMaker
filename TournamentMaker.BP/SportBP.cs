using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TournamentMaker.BO;
using TournamentMaker.DAL;

namespace TournamentMaker.BP
{
    public class SportBP
    {
        private readonly IUnityContainer _unityContainer;
        public SportBP(IUnityContainer unityContainer)
        {
            if (unityContainer == null) throw new ArgumentNullException("unityContainer");

            _unityContainer = unityContainer;
        }
        public async Task<ICollection<Sport>> Get()
        {
            using (var matchContext = _unityContainer.Resolve<MatchContext>())
            {
                ICollection<Sport> sports = await matchContext.Sports.ToListAsync();
                return sports;
            }
        }
    }
}
