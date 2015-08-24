using System.Configuration;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Microsoft.Practices.Unity.Configuration;
using TournamentMaker.Infrastructure.Mvc;
using TournamentMaker.Infrastructure.WebApi;

namespace TournamentMaker
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            // MVC dependency resolver
            DependencyResolver.SetResolver(new UnityMvcDependencyResolver(container));

            // WebApi
            GlobalConfiguration.Configuration.DependencyResolver = new UnityWebApiDependencyResolver(container);
        }
    }
}