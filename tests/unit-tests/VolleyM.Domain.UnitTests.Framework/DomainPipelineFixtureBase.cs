using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class DomainPipelineFixtureBase : IDisposable
    {
        private const string TEST_TARGET_KEY = "TestTarget";

        private IConfiguration _configuration;

        protected TestTarget Target { get; private set; }

        protected DomainPipelineFixtureBase()
        {
            InitConfiguration();

            ConfigureLogger();

            Log.Information("Test run started");

            Log.Information("Test is started for {Target}.", Target);
        }

        private void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test-config.json", true)
                .AddEnvironmentVariables("VOLLEYM_");

            _configuration = builder.Build();

            Target = ReadTarget();
        }

        private TestTarget ReadTarget()
        {
            var result = TestTarget.Unit;

            var targetString = _configuration[TEST_TARGET_KEY];

            if (!string.IsNullOrWhiteSpace(targetString))
            {
                if (!Enum.TryParse(targetString, false, out result))
                {
                    result = TestTarget.Unit;
                }
            }

            return result;
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .WriteTo.File(
                    "test-run.log",
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024/*10 MB*/,
                    retainedFileCountLimit: 10)
                .CreateLogger();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // no dispose in base
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers();

        internal IEnumerable<IAssemblyBootstrapper> GetBootstrappers() =>
            new List<IAssemblyBootstrapper>(GetAssemblyBootstrappers()) {
                new DomainContractsAssemblyBootstrapper()
            };
    }
}
