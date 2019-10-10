using System.Collections.Generic;
using System.Linq;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using Xunit;

namespace VolleyM.Domain.Framework.UnitTests
{
    public class DomainFrameworkFixture : DomainPipelineFixtureBase
    {
        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers()
        {
            return Enumerable.Empty<IAssemblyBootstrapper>();
        }
    }

    [CollectionDefinition("Domain framework")]
    public class DatabaseCollection : ICollectionFixture<DomainFrameworkFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("Domain framework")]
    public class DomainFrameworkStepsBase : DomainStepsBase<DomainFrameworkFixture>
    {
        public DomainFrameworkStepsBase(DomainFrameworkFixture fixture)
            : base(fixture)
        {
        }
    }
}