using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace TournamentMaker.Infrastructure.WebApi
{
    public class UnityWebApiDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer unityContainer;

        public UnityWebApiDependencyResolver(IUnityContainer unityContainer)
        {
            if (unityContainer == null) 
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void Dispose()
        {
            if (unityContainer != null)
            {
                unityContainer.Dispose();
            }
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return unityContainer.Resolve(serviceType);
            }
            catch
            {
                // Service locator spec
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return unityContainer.ResolveAll(serviceType);
            }
            catch
            {
                // Service locator spec
                return null;
            }
        }

        public IDependencyScope BeginScope()
        {
            IUnityContainer childContainer = unityContainer.CreateChildContainer();
            return new UnityWebApiDependencyResolver(childContainer);
        }
    }
}
