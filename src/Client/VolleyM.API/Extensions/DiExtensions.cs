using AutoMapper;
using AutoMapper.Configuration;
using Esquio.Abstractions;
using Microsoft.AspNetCore.Builder;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using VolleyM.Domain.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.API.Extensions
{
    internal static class DiExtensions
    {
        internal static Container CreateContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            //should be last
            container.Options.LifestyleSelectionBehavior = new VolleyMLifestyleSelectionBehavior(container.Options);

            return container;
        }

        /// <summary>
        /// Loads plugins, composes DI.
        /// </summary>
        public static void RegisterApplicationServices(
            this IApplicationBuilder app,
            Container container,
            AssemblyBootstrapper bootstrapper,
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            // Application Assemblies
            bootstrapper.RegisterDependencies(container, config);

            RegisterAutoMapper(container, bootstrapper);

            app.UseSimpleInjector(container, options =>
            {
                options.AutoCrossWireFrameworkComponents = false;

                options.CrossWire<IFeatureService>();
            });
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