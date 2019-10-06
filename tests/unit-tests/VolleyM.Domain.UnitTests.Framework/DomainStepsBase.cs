﻿using System;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.UnitTests.Framework
{
    public class DomainStepsBase<TFixture> : Feature, IDisposable
        where TFixture : DomainPipelineFixtureBase
    {
        private readonly Container _container;
        private readonly Scope _scope;

        protected DomainStepsBase(TFixture fixture)
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            foreach (var bootstrapper in fixture.GetBootstrappers())
            {
                bootstrapper.RegisterDependencies(_container, fixture.Configuration);
            }

            _scope = AsyncScopedLifestyle.BeginScope(_container);

            fixture.ApplyContainer(_container);
        }

        protected TService Resolve<TService>() where TService : class
            => _container.GetInstance<TService>();

        protected void Register<TInterface>(Func<TInterface> instanceCreator, Lifestyle lifestyle)
            where TInterface : class
        {
            _container.Register(instanceCreator, lifestyle);
        }

        ~DomainStepsBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
                _scope.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}