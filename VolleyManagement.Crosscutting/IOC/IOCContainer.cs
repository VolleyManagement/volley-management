using System;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace VolleyManagement.Crosscutting.IOC
{
    public class IOCContainer
    {
        private readonly Container _container;

        public IOCContainer()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        }

        public IDependencyResolver GetResolver()
        {
            return new SimpleInjectorDependencyResolver(_container);
        }

        public IOCContainer Register(IIOCRegistrationModule module)
        {
            module.RegisterDependencies(this);
            return this;
        }

        public IOCContainer RegisterSingleton<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Singleton);
            return this;
        }

        public IOCContainer RegisterSingleton(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Singleton);
            return this;
        }

        public IOCContainer RegisterScoped<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Scoped);
            return this;
        }

        public IOCContainer RegisterScoped(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Scoped);
            return this;
        }

        public IOCContainer RegisterTransient<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Transient);
            return this;
        }

        public IOCContainer RegisterTransient(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Transient);
            return this;
        }

        /*public IOCContainer Register(Type genericContract, Assembly[] assembliesToCheck)
        {
            _container.Register(genericContract, assembliesToCheck);
            return this;
        }*/

    }
}
