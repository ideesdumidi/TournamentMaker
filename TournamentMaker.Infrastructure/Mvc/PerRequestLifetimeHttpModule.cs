using System.Web;

namespace TournamentMaker.Infrastructure.Mvc
{
    public class PerRequestLifetimeHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, args) => UnityMvcDependencyResolver.DisposeChildContainer();
        }

        public void Dispose()
        {
            // rien à faire
        }
    }
}
