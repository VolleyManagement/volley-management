using AutoMapper.Configuration;
using FluentValidation;
using SimpleInjector;
using System.Composition;
using System.Linq;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.FeatureManagement;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.Framework.Logging;
using VolleyM.Domain.Framework.Validation;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Framework
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainFrameworkAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register<IAuthorizationHandler, DefaultAuthorizationHandler>(Lifestyle.Scoped);
            container.Register<IAuthorizationService, AuthorizationService>(Lifestyle.Scoped);

            container.Register<ICurrentUserProvider, CurrentUserProvider>(Lifestyle.Scoped);
            container.Register<ICurrentUserManager, CurrentUserProvider>(Lifestyle.Scoped);

            container.Register<HandlerMetadataService>(Lifestyle.Singleton);

            RegisterHandlerDecorators(container);

            RegisterQueryObjectDecorators(container);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no need for mappers
        }

        private static void RegisterHandlerDecorators(Container container)
        {
            // order is important. First decorator will wrap real instance
            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(ValidationHandlerDecorator<,>),
                Lifestyle.Scoped,
                context => DecorateWhenHasValidator(context, container));

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(AuthorizationHandlerDecorator<,>),
                Lifestyle.Scoped);

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(FeatureToggleDecorator<,>),
                Lifestyle.Scoped);

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(LoggingRequestHandlerDecorator<,>),
                Lifestyle.Scoped);
        }

        private static bool DecorateWhenHasValidator(DecoratorPredicateContext c, Container container)
        {
            var metadata = container.GetInstance<HandlerMetadataService>();

            var handlerType = c?.ImplementationType;

            return handlerType != null && metadata.HasValidator(handlerType);
        }

        private static void RegisterQueryObjectDecorators(Container container)
        {
            // order is important. First decorator will wrap real instance
            container.RegisterDecorator(
                typeof(IQuery<,>),
                typeof(LoggingQueryObjectDecorator<,>),
                Lifestyle.Scoped);
        }
    }
}