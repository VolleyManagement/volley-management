using System;
using System.Collections.Generic;
using BoDi;
using TechTalk.SpecFlow;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors.UnitTests
{
    [Binding]
    public class ContributorsStepsBase : DomainStepsBase
    {
        public ContributorsStepsBase(IObjectContainer objectContainer) : base(objectContainer)
        {
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            TestRunFixtureBase.BeforeTestRun();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            TestRunFixtureBase.AfterTestRun();
        }
        protected override ITestFixture CreateTestFixture(TestTarget target)
        {
            return target switch
            {
                TestTarget.Unit => new UnitContributorsTestFixture(),
                _ => throw new NotSupportedException()
            };
        }

        protected override bool RequiresAuthorizationFixture => true;

        protected override Type GetConcreteTestFixtureType => typeof(IContributorsTestFixture);

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target) =>
            new[] {
                new DomainContributorsAssemblyBootstrapper()
            };
    }
}