using System;
using SimpleInjector;
using SimpleInjector.Advanced;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework
{
    public class VolleyMLifestyleSelectionBehavior : ILifestyleSelectionBehavior
    {
        private readonly ContainerOptions _options;

        public VolleyMLifestyleSelectionBehavior(ContainerOptions options)
        {
            this._options = options;
        }

        public Lifestyle SelectLifestyle(Type implementationType)
        {
            if (implementationType.IsAssignableFrom(typeof(IEventHandler<>)))
            {
                return _options.DefaultScopedLifestyle;
            }

            return this._options.DefaultLifestyle;
        }

    }
}