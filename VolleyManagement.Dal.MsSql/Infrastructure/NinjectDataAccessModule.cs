namespace VolleyManagement.Data.MsSql.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Planning.Bindings;

    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Services;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Defines bindings for Service layer
    /// </summary>
    public class NinjectDataAccessModule : NinjectModule
    {
        private readonly Func<IContext, object> _scopeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDataAccessModule"/> class.
        /// </summary>
        /// <param name="scopeCallback"> The scope callback. </param>
        public NinjectDataAccessModule(Func<IContext, object> scopeCallback)
        {
            this._scopeCallback = scopeCallback;
        }

        /// <summary>
        /// Loads bindings
        /// </summary>
        public override void Load()
        {
            var configs = new List<IBindingConfiguration>
                              {
                                  this.Bind<IUnitOfWork>().To<VolleyUnitOfWork>().BindingConfiguration,
                                  this.Bind<ITournamentRepository>().To<TournamentRepository>().BindingConfiguration,
                                  this.Bind<IUserRepository>().To<UserRepository>().BindingConfiguration
                              };
            if (this._scopeCallback != null)
            {
                configs.ForEach(bc => bc.ScopeCallback = this._scopeCallback);
            }
        }
    }
}
