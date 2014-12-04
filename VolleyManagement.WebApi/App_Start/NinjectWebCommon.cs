[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VolleyManagement.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(VolleyManagement.WebApi.App_Start.NinjectWebCommon), "Stop")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCopPlus.StyleCopPlusRules",
    "SP0100:AdvancedNamingRules", Justification = "Ninject.")]

namespace VolleyManagement.WebApi.App_Start
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using VolleyManagement.WebApi.Infrastructure;
    using System.Web.Http;

    using VolleyManagement.Services.Infrastructure;
    using VolleyManagement.Dal.MsSql.Infrastructure;

    /// <summary>
    /// Registers HttpModules
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Ninject")]
    public static class NinjectWebCommon
    {
        /// <summary>
        /// Bootstrapper bootstrapper
        /// </summary>
        private static readonly Bootstrapper _bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            _bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(
                new NinjectDataAccessModule(),
                new NinjectServiceBindModule()
                );
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }
    }
}
