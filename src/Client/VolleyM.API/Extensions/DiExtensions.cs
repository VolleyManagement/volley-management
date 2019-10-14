using AutoMapper;
using AutoMapper.Configuration;
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
        public static void RegisterApplicationServices(
            this Container container, 
            AssemblyBootstrapper bootstrapper, 
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            // Application Assemblies
            bootstrapper.RegisterDependencies(container, config);

            RegisterAutoMapper(container, bootstrapper);
        }

        private static void RegisterAutoMapper(Container container, AssemblyBootstrapper bootstrapper)
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(container.GetInstance);

            bootstrapper.RegisterMappingProfiles(mce);

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            container.RegisterSingleton<IMapper>(() => new Mapper(mc, container.GetInstance));
        }
    }
}