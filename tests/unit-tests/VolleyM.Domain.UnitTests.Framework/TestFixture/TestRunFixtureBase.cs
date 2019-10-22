using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace VolleyM.Domain.UnitTests.Framework
{
    /// <summary>
    /// Class used to manage and instantiate Test Run 
    /// </summary>
    public class TestRunFixtureBase
    {
        private const string TEST_TARGET_KEY = "TestTarget";
        private const string TEST_LOG_KEY = "TestLogName";

        internal static IConfiguration Configuration { get; set; }

        internal static TestTarget Target { get; private set; }

        private static IOneTimeTestFixture OneTimeFixture { get; set; }

        public static Func<TestTarget, IOneTimeTestFixture> OneTimeFixtureCreator { get; set; }

        public static void BeforeTestRun()
        {
            InitConfiguration();

            ConfigureLogger(Configuration);

            Log.Information("Test run started");
            Log.Information("Test is started for {Target}.", Target);

            SetupOneTimeFixture();
        }

        public static void AfterTestRun()
        {
            OneTimeFixture?.OneTimeTearDown();
        }

        private static void SetupOneTimeFixture()
        {
            if (OneTimeFixtureCreator == null) return;

            OneTimeFixture = OneTimeFixtureCreator(Target);
            OneTimeFixture.OneTimeSetup(Configuration);
        }

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
    }
}