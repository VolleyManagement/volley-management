using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using Xunit;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    public class IdentityAndAccessFixture : DomainPipelineFixtureBase, IIdentityAndAccessFixture
    {
        private readonly IIdentityAndAccessFixture _testFixture;

        public IdentityAndAccessFixture()
        {
            _testFixture = (IIdentityAndAccessFixture) CreateTestFixture(Target);
        }

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers() =>
            new[] {
                new DomainIdentityAndAccessAssemblyBootstrapper()
            };

        private ITestFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => new UnitTestIdentityAndAccessFixture(this),
                TestTarget.AzureCloud => new UnitTestIdentityAndAccessFixture(this),// Same for now
                TestTarget.OnPremSql => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
        }

        public Task Initialize()
        {
            return _testFixture.Initialize();
        }

        public Task ConfigureUserExists(UserId id, User user)
        {
            return _testFixture.ConfigureUserExists(id, user);
        }

        public Task ConfigureUserDoesNotExist(UserId id)
        {
            return _testFixture.ConfigureUserDoesNotExist(id);
        }

        public Task VerifyUserCreated(User user)
        {
            return _testFixture.VerifyUserCreated(user);
        }
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