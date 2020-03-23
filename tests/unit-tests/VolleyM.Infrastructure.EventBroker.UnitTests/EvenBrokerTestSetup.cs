using System;
using System.Collections.Generic;
using System.Linq;
using BoDi;
using TechTalk.SpecFlow;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    [Binding]
    public class EvenBrokerTestSetup : DomainTestSetupBase
    {
        public EvenBrokerTestSetup(IObjectContainer objectContainer) : base(objectContainer) { }

        [BeforeTestRun]
        public static void OneTimeSetup()
        {
            TestRunFixtureBase.BeforeTestRun();
        }

        [AfterTestRun]
        public static void OneTimeTearDown()
        {
            TestRunFixtureBase.AfterTestRun();
        }

        protected override ITestFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => new UnitEventBrokerTestFixture(),
                _ => throw new NotSupportedException()
            };
        }

        protected override bool RequiresAuthorizationFixture => false;

        protected override Type GetConcreteTestFixtureType => typeof(IEventBrokerTestFixture);

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target)
        {
            return Enumerable.Empty<IAssemblyBootstrapper>();
        }
    }
}