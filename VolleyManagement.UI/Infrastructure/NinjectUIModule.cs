namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using Contracts;
    using Contracts.Authorization;
    using Ninject.Activation;
    using Ninject.Infrastructure;
    using Ninject.Modules;
    using Services;

    /// <summary>
    /// Defines bindings for UI layer
    /// </summary>
    public class NinjectUIModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectUIModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectUIModule(Func<IContext, object> scopeCallback)
        {
            _scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            IHaveBindingConfiguration configuration = Bind<ICurrentUserService>().To<CurrentUserService>();
            configuration.BindingConfiguration.ScopeCallback = _scopeCallback;
            IHaveBindingConfiguration configurationCaptcha = Bind<ICaptchaManager>().To<CaptchaManager>();
            configurationCaptcha.BindingConfiguration.ScopeCallback = _scopeCallback;
            IHaveBindingConfiguration configurationFileService = Bind<IFileService>().To<FileService>();
            configurationFileService.BindingConfiguration.ScopeCallback = _scopeCallback;
        }
    }
}
