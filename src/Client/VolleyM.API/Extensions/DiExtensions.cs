using SimpleInjector;
using SimpleInjector.Lifestyles;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API.Extensions
{
    internal static class DiExtensions
    {
        internal static Container CreateContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            return container;
        }

        /// <summary>
        /// Loads plugins, composes DI.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pluginLocation"></param>
        public static void RegisterApplicationServices(this Container container, string pluginLocation)
        {
            var bootstrapper = new AssemblyBootstrapper();
            bootstrapper.Compose(container, pluginLocation);
        }
    }
}