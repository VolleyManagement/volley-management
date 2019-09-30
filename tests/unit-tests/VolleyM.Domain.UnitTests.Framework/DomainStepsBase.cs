﻿using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.UnitTests.Framework
{
    public class DomainStepsBase<TFixture> : Feature
        where TFixture : DomainPipelineFixtureBase
    {
        private readonly Container _container = new Container();
        private readonly Scope _scope;

        protected DomainStepsBase(TFixture fixture)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            foreach (var bootstrapper in fixture.GetBootstrappers())
            {
                bootstrapper.RegisterDependencies(_container);
            }

            _scope = AsyncScopedLifestyle.BeginScope(_container);
        }

        protected TService Resolve<TService>() where TService : class
            => _container.GetInstance<TService>();

        protected void Register<TInterface>(Func<TInterface> instanceCreator, Lifestyle lifestyle)
            where TInterface : class
        {
            _container.Register(instanceCreator, lifestyle);
        }

        ~DomainStepsBase() => _scope.Dispose();
    }
}