using AutoMapper;
using AutoMapper.Configuration;
using BoDi;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class DomainStepsBase
    {
        private readonly IObjectContainer _objectContainer;

        #region Scenario variables

        protected Container Container { get; private set; }

        private ITestFixture BaseTestFixture { get; set; }

        private IAuthFixture AuthFixture { get; set; }

        private Scope _scope;

        protected DomainStepsBase(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        #endregion

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_TEST_FRAMEWORK_ORDER)]
        public void BeforeEachScenario()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
            // Allow some tests to override existing registrations to replace it with test doubles
            Container.Options.AllowOverridingRegistrations = true;

            RegisterContainerInSpecFlow(Container);

            RegisterAssemblyBootstrappers();

            BaseTestFixture = CreateAndRegisterTestFixtureIfNeeded(TestRunFixtureBase.Target);
            AuthFixture = CreateAndRegisterAuthFixtureIfNeeded();

            AuthFixture.ConfigureTestUserRole(Container);
            BaseTestFixture.RegisterScenarioDependencies(Container);

            _scope = ThreadScopedLifestyle.BeginScope(Container);

            AuthFixture.ConfigureTestUser(Container);

            BaseTestFixture.ScenarioSetup();
        }

        [AfterScenario(Order = Constants.AFTER_SCENARIO_TEST_FRAMEWORK_ORDER)]
        public void AfterEachScenario()
        {
            BaseTestFixture.ScenarioTearDown();

            _scope.Dispose();
            _scope = null;
            Container.Dispose();
            Container = null;
        }

        /// <summary>
        /// Indicates whether default Authentication should be configured or test fixture will handle it.
        /// </summary>
        /// <remarks>Almost all Domain specific tests need this</remarks>
        protected abstract bool RequiresAuthorizationFixture { get; }

        protected abstract IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget testTarget);

        /// <summary>
        /// Returns interface type of concrete ITestFixture
        /// </summary>
        protected virtual Type GetConcreteTestFixtureType => typeof(NoOpTestFixture);

        protected virtual ITestFixture CreateTestFixture(TestTarget testTarget)
        {
            return new NoOpTestFixture();
        }

        #region Test Configuration

        private void RegisterAssemblyBootstrappers()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(Container.GetInstance);

            foreach (var bootstrapper in GetBootstrappers())
            {
                bootstrapper.RegisterDependencies(Container, TestRunFixtureBase.Configuration);
                bootstrapper.RegisterMappingProfiles(mce);
            }

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            Container.RegisterSingleton<IMapper>(() => new Mapper(mc, Container.GetInstance));
        }

        private IEnumerable<IAssemblyBootstrapper> GetBootstrappers() =>
            new List<IAssemblyBootstrapper>(GetAssemblyBootstrappers(TestRunFixtureBase.Target)) {
                new DomainFrameworkAssemblyBootstrapper()
            };

        private ITestFixture CreateAndRegisterTestFixtureIfNeeded(TestTarget target)
        {
            var result = CreateTestFixture(target);

            var fixtureType = GetConcreteTestFixtureType;
            if (fixtureType != typeof(NoOpTestFixture))
            {
                _objectContainer.RegisterInstanceAs(result, fixtureType);
            }

            return result;
        }

        private IAuthFixture CreateAndRegisterAuthFixtureIfNeeded()
        {
            if (!RequiresAuthorizationFixture) return null;

            var fixture = new AuthFixture();

            _objectContainer.RegisterInstanceAs(fixture, typeof(IAuthFixture));

            return fixture;
        }

        private void RegisterContainerInSpecFlow(Container container)
        {
            _objectContainer.RegisterInstanceAs(container);
        }

        #endregion
    }
}