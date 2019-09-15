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
        public DomainPipelineFixtureBase()
        {
            ConfigureLogger();
        }

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .WriteTo.Console(LogEventLevel.Information)
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
