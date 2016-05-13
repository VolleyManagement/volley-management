using VolleyManagement.UI;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace VolleyManagement.UI
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Web.Common;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using Ninject.Web.WebApi;

    using VolleyManagement.Data.MsSql.Infrastructure;
    using VolleyManagement.Services.Infrastructure;
    using VolleyManagement.UI.Infrastructure;

    /// <summary>
    /// Provides IoC for ASP.NET MVC
    /// </summary>
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper _bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            _bootstrapper.Initialize(CreateKernel);

            // Set resolver for Api controllers
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(_bootstrapper.Kernel);
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
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            RegisterInfrastructure(kernel);

            var scope = GetPerRequestScopeCallback();

            kernel.Load(new NinjectDataAccessModule(scope));
            kernel.Load(new NinjectServiceBindModule(scope));
            kernel.Load(new NinjectUIModule(scope));
        }

        private static Func<IContext, object> GetPerRequestScopeCallback()
        {
            var kernel = new StandardKernel();

            // This is dirty hack to get Request Scope callback
            var result = kernel.Bind<string>().ToSelf().InRequestScope().BindingConfiguration.ScopeCallback;
            return result;
        }

        private static void RegisterInfrastructure(IKernel kernel)
        {
            kernel.BindFilter<VolleyExceptionFilterAttribute>(FilterScope.Global, 0);
        }
    }
}
