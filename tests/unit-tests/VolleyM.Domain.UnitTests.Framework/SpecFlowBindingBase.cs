using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using AutoMapper.Configuration;
using FluentAssertions;
using SimpleInjector.Lifestyles;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class SpecFlowBindingBase
    {
        private const string TEST_TARGET_KEY = "TestTarget";
        private const string TEST_LOG_KEY = "TestLogName";


        public const int ONETIME_FRAMEWORK_ORDER = 200;
        public const int ONETIME_DOMAIN_FIXTURE_ORDER = 100;

        private static IConfiguration Configuration { get; set; }
        public Container Container { get; set; }
        private Scope _scope;

        protected static TestTarget Target { get; private set; }

        protected static IOneTimeTestFixture OneTimeFixture { get; private set; }

        protected static Func<TestTarget, IOneTimeTestFixture> OneTimeFixtureCreator { get; set; }

        [BeforeTestRun(Order = ONETIME_FRAMEWORK_ORDER)]
        public static void BeforeTestRun()
        {
            InitConfiguration();

            ConfigureLogger(Configuration);

            Log.Information("Test run started");
            Log.Information("Test is started for {Target}.", Target);

            OneTimeFixture = OneTimeFixtureCreator(Target);

            OneTimeFixture.OneTimeSetup(Configuration);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            OneTimeFixture.OneTimeTearDown();
        }

        [BeforeScenario]
        public virtual void BeforeEachScenario()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();


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

            _scope = AsyncScopedLifestyle.BeginScope(Container);
        }

        [AfterScenario]
        public virtual void AfterEachScenario()
        {
            _scope.Dispose();
            _scope = null;
            Container.Dispose();
            Container = null;
        }

        protected abstract IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers();

        internal IEnumerable<IAssemblyBootstrapper> GetBootstrappers() =>
            new List<IAssemblyBootstrapper>(GetAssemblyBootstrappers()) {
                new DomainFrameworkAssemblyBootstrapper()
            };

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

        protected void AssertErrorReturned<T>(
            Result<T> actualResult,
            Error expected,
            string because = "error should be reported",
            params object[] becauseArgs) where T : class
        {
            actualResult.Error.Should().BeEquivalentTo(expected, because, becauseArgs);
        }
    }
}