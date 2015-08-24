using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TournamentMaker.BO;
using TournamentMaker.DAL;

namespace TournamentMaker.BP
{
    public class RankBP
    {
        private readonly IUnityContainer _unityContainer;
        public RankBP(IUnityContainer unityContainer)
        {
            if (unityContainer == null) throw new ArgumentNullException("unityContainer");

            _unityContainer = unityContainer;
        }
        public async Task<ICollection<Rank>> Get()
        {
            using (var matchContext = _unityContainer.Resolve<MatchContext>())
            {
                var ranks = await matchContext.Players.Select(p=>new {p=p,r=p.Ranks.Sum(r=>r.Level)}).OrderByDescending(r=>r.r).ToListAsync();
                return ranks.Select(r=>new Rank{Player = r.p,Level= r.r}).ToList();
            }
        }
        public async Task<ICollection<Rank>> Get(string sportKey)
        {
            using (var matchContext = _unityContainer.Resolve<MatchContext>())
            {
                ICollection<Rank> ranks = await matchContext.Ranks.Where(r => r.SportKey == sportKey).Include("Player").OrderByDescending(r => r.Level).ToListAsync();
                return ranks;
            }
        }
    }
}
