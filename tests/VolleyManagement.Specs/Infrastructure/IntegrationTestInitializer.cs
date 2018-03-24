using AutoMapper;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyManagement.Data.MsSql.Context;
using VolleyManagement.Specs.Infrastructure.IOC;
using VolleyManagement.Specs.TestFixture;

namespace VolleyManagement.Specs.Infrastructure
{
    [Binding]
    public static class IntegrationTestInitializer
    {
        private static Scope _asyncScope;

        [BeforeTestRun]
        public static void BootstrapApplication()
        {
            InitializeInfrastructureForIntegrationTest();

            SetDbToInitialState();
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            _asyncScope = IocProvider.Instance.BeginScope();
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            TestDbAdapter.Respawn();

            EndIocScope();
        }

        private static void EndIocScope()
        {
            _asyncScope?.Dispose();
            _asyncScope = null;
        }

        private static void InitializeInfrastructureForIntegrationTest()
        {
            // AutoMapper config
            Mapper.Initialize(cfg => { });

            // Provide connection string for integration test DB
            VolleyManagementDbContextFactory.ConnectionNameOrString =
                IntegrationTestConfigurationProvider.IT_CONNECTION_STRING;

            // Consider a first test - make sure IoC dependencies can be resolved
            IocProvider.Instance.Verify();
        }

        private static void SetDbToInitialState()
        {
            InitialStateTestFixture.ConfigureTestRun();
        }
    }
}