using FluentValidation;
using SimpleInjector;
using System.Collections.Generic;
using System.Reflection;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Framework
{
    public class FrameworkDomainComponentDependencyRegistrar : IDomainComponentDependencyRegistrar
    {
        public void RegisterCommonDependencies(Container container, List<Assembly> domainComponentsAssemblies)
        {
            RegisterCommonServices(container, domainComponentsAssemblies);
        }

        public static void RegisterCommonServices(Container container, List<Assembly> domainComponentsAssemblies)
        {
            container.Register(typeof(IRequestHandler<,>), domainComponentsAssemblies, Lifestyle.Scoped);

            container.Collection.Register(typeof(IEventHandler<>), domainComponentsAssemblies);
            container.Collection.Register(typeof(IEventHandler), domainComponentsAssemblies);

            container.Register(typeof(IValidator<>), domainComponentsAssemblies, Lifestyle.Scoped);
        }
    }
}