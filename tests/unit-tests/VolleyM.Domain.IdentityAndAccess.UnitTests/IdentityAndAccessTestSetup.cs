using BoDi;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.Hardcoded;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [Binding]
    public class IdentityAndAccessTestSetup : DomainTestSetupBase
    {
        public IdentityAndAccessTestSetup(IObjectContainer objectContainer) : base(objectContainer) { }

        #region One Time Fixture Setup

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            TestRunFixtureBase.OneTimeFixtureCreator = CreateOneTimeTestFixture;
            TestRunFixtureBase.BeforeTestRun();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            TestRunFixtureBase.AfterTestRun();
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

        #endregion

        #region Test Fixture Setup

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

        protected override bool RequiresAuthorizationFixture => true;

        protected override Type GetConcreteTestFixtureType => typeof(IIdentityAndAccessFixture);

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target)
        {
            var result = new List<IAssemblyBootstrapper> { new DomainIdentityAndAccessAssemblyBootstrapper() };

            if (target == TestTarget.AzureCloud)
            {
                result.Add(new InfrastructureIdentityAndAccessAzureStorageBootstrapper());
                result.Add(new InfrastructureHardcodedAssemblyBootstrapper());
            }

            return result;
        }

        #endregion
    }
}