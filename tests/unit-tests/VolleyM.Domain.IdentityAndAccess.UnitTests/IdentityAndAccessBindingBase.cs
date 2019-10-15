using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    public class IdentityAndAccessBindingBase : SpecFlowBindingBase
    {
        protected IIdentityAndAccessFixture Fixture { get; private set; }

        [BeforeTestRun(Order = ONETIME_DOMAIN_FIXTURE_ORDER)]
        public static void OneTimeSetup()
        {
            OneTimeFixtureCreator = CreateOneTimeTestFixture;
            SpecFlowBindingBase.BeforeTestRun();
        }

        [AfterTestRun]
        public static void OneTimeTearDown()
        {
            SpecFlowBindingBase.AfterTestRun();
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

        public override void BeforeEachScenario()
        {
            base.BeforeEachScenario();

            Fixture = CreateTestFixture(Target);
            Fixture.Setup();
        }

        private IIdentityAndAccessFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => (IIdentityAndAccessFixture)new UnitTestIdentityAndAccessFixture(Container),
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