using System.Reflection;
using FluentValidation;
using SimpleInjector;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public static class RegistrationExtensions
    {
        public static void RegisterCommonDomainServices(this Container container, Assembly assembly)
        {
            container.Register(typeof(IRequestHandler<,>), assembly, Lifestyle.Scoped);
            container.Register(typeof(IEventHandler<>), assembly, Lifestyle.Scoped);

            container.Register(typeof(IValidator<>), assembly, Lifestyle.Scoped);
        }
    }
}