using AutoMapper;
using AutoMapper.Configuration;
using Destructurama;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class SpecFlowBindingBase
    {
        #region Constants

        private const string TEST_TARGET_KEY = "TestTarget";
        private const string TEST_LOG_KEY = "TestLogName";

        private static readonly RoleId _testUserRoleId = new RoleId("testuser@volleym.idp");

        #endregion

        #region Test Run variables

        private static IConfiguration Configuration { get; set; }

        protected static TestTarget Target { get; private set; }

        private static IOneTimeTestFixture OneTimeFixture { get; set; }

        protected static Func<TestTarget, IOneTimeTestFixture> OneTimeFixtureCreator { get; set; }

        #endregion

        #region Scenario variables

        protected Container Container { get; private set; }

        protected ITestFixture BaseTestFixture { get; private set; }

        private Scope _scope;

        private Role _testUserRole;

        #endregion

        protected static void BeforeTestRun()
        {
            InitConfiguration();

            ConfigureLogger(Configuration);

            Log.Information("Test run started");
            Log.Information("Test is started for {Target}.", Target);

            OneTimeFixture = OneTimeFixtureCreator(Target);

            OneTimeFixture.OneTimeSetup(Configuration);
        }

        protected static void AfterTestRun()
        {
            OneTimeFixture.OneTimeTearDown();
        }

        [BeforeScenario]
        public void BeforeEachScenario()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            // Allow some tests to override existing registrations to replace it with test doubles
            Container.Options.AllowOverridingRegistrations = true;

            RegisterAssemblyBootstrappers();

            BaseTestFixture = CreateTestFixture(Target);
            
            MockTestAuthorization();
            RegisterDependenciesForScenario(Container);

            _scope = AsyncScopedLifestyle.BeginScope(Container);

            ScenarioSetup();
        }

        [AfterScenario]
        public void AfterEachScenario()
        {
            ScenarioTearDown();

            _scope.Dispose();
            _scope = null;
            Container.Dispose();
            Container = null;
        }

        protected virtual ITestFixture CreateTestFixture(TestTarget testTarget)
        {
            return new NoOpTestFixture();
        }

        /// <summary>
        /// Hook to register dependencies for each scenario. Called before <see cref="ScenarioSetup"/>
        /// </summary>
        /// <param name="container"></param>
        protected virtual void RegisterDependenciesForScenario(Container container)
        {
            BaseTestFixture.RegisterScenarioDependencies(container);
        }

        /// <summary>
        /// Logic to run to set up scenario. Called after <see cref="RegisterDependenciesForScenario"/>
        /// </summary>
        protected virtual void ScenarioSetup()
        {
            BaseTestFixture.ScenarioSetup();
        }

        /// <summary>
        /// Hook to run scenario clean up
        /// </summary>
        protected virtual void ScenarioTearDown()
        {
            BaseTestFixture.ScenarioTearDown();
        }

        protected abstract IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers();

        private IEnumerable<IAssemblyBootstrapper> GetBootstrappers() =>
            new List<IAssemblyBootstrapper>(GetAssemblyBootstrappers()) {
                new DomainFrameworkAssemblyBootstrapper()
            };

        #region Test Configuration

        private static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test-config.json", true)
                .AddEnvironmentVariables("VOLLEYM_");

            Configuration = builder.Build();

            Target = ReadTarget();
        }

        private static TestTarget ReadTarget()
        {
            var result = TestTarget.Unit;

            var targetString = Configuration[TEST_TARGET_KEY];

            if (!string.IsNullOrWhiteSpace(targetString))
            {
                if (!Enum.TryParse(targetString, true, out result))
                {
                    result = TestTarget.Unit;
                }
                else
                {
                    Log.Warning("Failed to parse test target string. {TargetString}", targetString);
                }
            }

            return result;
        }

        private static void ConfigureLogger(IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Destructure.UsingAttributes()
                .WriteTo.Debug()
                .WriteTo.File(
                    GetLogName(config),
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024/*10 MB*/,
                    retainedFileCountLimit: 10)
                .CreateLogger();
        }

        private static string GetLogName(IConfiguration config)
        {
            return config[TEST_LOG_KEY] ?? "test-run.log";
        }

        private void RegisterAssemblyBootstrappers()
        {
            var mce = new MapperConfigurationExpression();
            mce.ConstructServicesUsing(Container.GetInstance);

            foreach (var bootstrapper in GetBootstrappers())
            {
                bootstrapper.RegisterDependencies(Container, Configuration);
                bootstrapper.RegisterMappingProfiles(mce);
            }

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            Container.RegisterSingleton<IMapper>(() => new Mapper(mc, Container.GetInstance));
        }

        #endregion

        private void MockTestAuthorization()
        {
            var store = Substitute.For<IRolesStore>();

            _testUserRole = new Role(_testUserRoleId);
            store.Get(_testUserRoleId).Returns(_testUserRole);

            Container.Register(() => store, Lifestyle.Scoped);
        }

        protected void ConfigurePermission(Permission permission)
        {
            _testUserRole.AddPermission(permission);
        }

        protected void AssertErrorReturned<T>(
            Result<T> actualResult,
            Error expected,
            string because = "error should be reported",
            params object[] becauseArgs) where T : class
        {
            actualResult.Should().NotBeSuccessful("error is expected");
            actualResult.Error.Should().BeEquivalentTo(expected, because, becauseArgs);
        }
    }
}