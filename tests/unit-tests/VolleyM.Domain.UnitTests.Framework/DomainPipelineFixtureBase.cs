using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Events;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.UnitTests.Framework
{
    public abstract class DomainPipelineFixtureBase : IDisposable
    {
        protected DomainPipelineFixtureBase()
        {
            ConfigureLogger();

            Log.Information("Test run started");
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
