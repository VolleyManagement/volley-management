namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using Ninject.Activation;
    using Ninject.Modules;
    using VolleyManagement.Contracts.Authorization;
    using Ninject.Infrastructure;

    public class NinjectUIModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectUIModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectUIModule(Func<IContext, object> scopeCallback)
        {
            this._scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            IHaveBindingConfiguration configuration = Bind<ICurrentUserService>().To<CurrentUserService>();
            configuration.BindingConfiguration.ScopeCallback = _scopeCallback;
        }
    }


}
