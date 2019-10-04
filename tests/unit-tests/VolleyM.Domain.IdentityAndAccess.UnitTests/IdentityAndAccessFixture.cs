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
            _testFixture = (IIdentityAndAccessFixture)CreateTestFixture(Target);
        }

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers()
        {
            var result = new List<IAssemblyBootstrapper> { new DomainIdentityAndAccessAssemblyBootstrapper() };

            if (Target == TestTarget.AzureCloud)
            {
                //result.Add(new InfrastructureIdentityAndAccessAzureStorageAssemblyBootstrapper());
            }

            return result;
        }

        private ITestFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => (ITestFixture)new UnitTestIdentityAndAccessFixture(this),
                TestTarget.AzureCloud => new AzureCloudIdentityAndAccessFixture(this),
                TestTarget.OnPremSql => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
        }

        public void Initialize()
        {
            _testFixture.Initialize();
        }

        public void ConfigureUserExists(UserId id, User user)
        {
            _testFixture.ConfigureUserExists(id, user);
        }

        public void ConfigureUserDoesNotExist(UserId id)
        {
            _testFixture.ConfigureUserDoesNotExist(id);
        }

        public void VerifyUserCreated(User user)
        {
            _testFixture.VerifyUserCreated(user);
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