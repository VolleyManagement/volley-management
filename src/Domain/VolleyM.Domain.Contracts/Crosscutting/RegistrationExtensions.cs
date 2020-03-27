using System.Reflection;
using FluentValidation;
using SimpleInjector;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public static class RegistrationExtensions
    {
        public static void RegisterCommonDomainServices(this Container container, Assembly assembly)
        {
            container.Register(typeof(IRequestHandler<,>), assembly, Lifestyle.Scoped);

            container.Collection.Register(typeof(IEventHandler<>), assembly);

            container.Register(typeof(IValidator<>), assembly, Lifestyle.Scoped);
        }
    }
}