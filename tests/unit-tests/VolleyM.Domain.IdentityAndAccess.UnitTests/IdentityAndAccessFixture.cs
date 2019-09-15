using System.Collections.Generic;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using Xunit;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    public class IdentityAndAccessFixture : DomainPipelineFixtureBase
    {
        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers() =>
            new [] {
                new DomainIdentityAndAccessAssemblyBootstrapper()
            };
    }

    [CollectionDefinition("Identity And Access domain")]
    public class DatabaseCollection : ICollectionFixture<IdentityAndAccessFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("Identity And Access domain")]
    public class IdentityAndAccessStepsBase : DomainStepsBase<IdentityAndAccessFixture>
    {
        public IdentityAndAccessStepsBase(IdentityAndAccessFixture fixture)
            : base(fixture)
        {
        }
    }
}