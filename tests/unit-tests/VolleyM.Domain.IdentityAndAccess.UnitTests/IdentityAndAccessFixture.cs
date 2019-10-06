using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage;
using Xunit;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    public class IdentityAndAccessFixture : DomainPipelineFixtureBase, IIdentityAndAccessFixture
    {
        private readonly IIdentityAndAccessFixture _testFixture;

        public IdentityAndAccessFixture()
        {
            _testFixture = (IIdentityAndAccessFixture)CreateTestFixture(Target);

            _testFixture.OneTimeSetup(Configuration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _testFixture.OneTimeTearDown();
            }
        }

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers()
        {
            var result = new List<IAssemblyBootstrapper> { new DomainIdentityAndAccessAssemblyBootstrapper() };

            if (Target == TestTarget.AzureCloud)
            {
                result.Add(new InfrastructureIdentityAndAccessAzureStorageBootstrapper());
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

        public void OneTimeSetup(IConfiguration configuration)
        {
            _testFixture.OneTimeSetup(configuration);
        }

        public void OneTimeTearDown()
        {
            _testFixture.OneTimeTearDown();
        }

        public void Setup()
        {
            _testFixture.Setup();
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            _testFixture.ConfigureUserExists(tenant, id, user);
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            _testFixture.ConfigureUserDoesNotExist(tenant, id);
        }

        public void VerifyUserCreated(User user)
        {
            _testFixture.VerifyUserCreated(user);
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            _testFixture.CleanUpUsers(usersToTeardown);
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