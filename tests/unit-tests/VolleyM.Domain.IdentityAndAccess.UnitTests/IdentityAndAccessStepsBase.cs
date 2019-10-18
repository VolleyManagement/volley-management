using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    public class IdentityAndAccessStepsBase : SpecFlowBindingBase
    {
        protected IIdentityAndAccessFixture Fixture => (IIdentityAndAccessFixture)BaseTestFixture;

        [BeforeTestRun]
        public static void OneTimeSetup()
        {
            OneTimeFixtureCreator = CreateOneTimeTestFixture;
            BeforeTestRun();
        }

        [AfterTestRun]
        public static void OneTimeTearDown()
        {
            AfterTestRun();
        }

        private static IOneTimeTestFixture CreateOneTimeTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => NoOpOneTimeTestFixture.Instance,
                TestTarget.AzureCloud => new AzureCloudIdentityAndAccessOneTimeFixture(),
                TestTarget.OnPremSql => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
        }

        protected override ITestFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => (IIdentityAndAccessFixture)new UnitTestIdentityAndAccessFixture(),
                TestTarget.AzureCloud => new AzureCloudIdentityAndAccessFixture(Container),
                TestTarget.OnPremSql => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
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
    }
}