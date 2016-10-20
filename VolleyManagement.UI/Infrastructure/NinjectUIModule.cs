namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using Ninject.Activation;
    using Ninject.Infrastructure;
    using Ninject.Modules;

    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;

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
            this._scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            IHaveBindingConfiguration configuration = Bind<ICurrentUserService>().To<CurrentUserService>();
            IHaveBindingConfiguration _userServiceBindingConfiguration =
                Bind<IUserService>().To<UserService>();

            configuration.BindingConfiguration.ScopeCallback = _scopeCallback;
            _userServiceBindingConfiguration.BindingConfiguration.ScopeCallback
                = _scopeCallback;
        }
    }
}
