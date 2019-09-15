using System.Collections.Generic;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using Xunit;

namespace VolleyM.Domain.Contributors.UnitTests
{
    public class ContributorsFixture : DomainPipelineFixtureBase
    {
        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers() =>
            new[] {
                new DomainContributorsAssemblyBootstrapper()
            };
    }

    [CollectionDefinition("Contributors domain")]
    public class DatabaseCollection : ICollectionFixture<ContributorsFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("Contributors domain")]
    public class ContributorsStepsBase : DomainStepsBase<ContributorsFixture>
    {
        public ContributorsStepsBase(ContributorsFixture fixture)
            : base(fixture)
        {
        }
    }
}