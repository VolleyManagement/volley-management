using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class DomainPipelineFixtureBase : IDisposable
    {
        private const string TEST_TARGET_KEY = "TestTarget";

        public IConfiguration Configuration { get; private set; }

        private Container _container;

        protected TestTarget Target { get; private set; }

        protected DomainPipelineFixtureBase()
        {
            InitConfiguration();

            ConfigureLogger();

            Log.Information("Test run started");
            Log.Information("Test is started for {Target}.", Target);
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

        internal void ApplyContainer(Container container)
        {
            _container = container;
        }

        public TService Resolve<TService>() where TService : class
            => _container.GetInstance<TService>();

        public void Register<TInterface>(Func<TInterface> instanceCreator, Lifestyle lifestyle)
            where TInterface : class
        {
            _container.Register(instanceCreator, lifestyle);
        }

        private void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test-config.json", true)
                .AddEnvironmentVariables("VOLLEYM_");

            Configuration = builder.Build();

            Target = ReadTarget();
        }

        private TestTarget ReadTarget()
        {
            var result = TestTarget.Unit;

            var targetString = Configuration[TEST_TARGET_KEY];

            if (!string.IsNullOrWhiteSpace(targetString))
            {
                if (!Enum.TryParse(targetString, false, out result))
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
    }
}
