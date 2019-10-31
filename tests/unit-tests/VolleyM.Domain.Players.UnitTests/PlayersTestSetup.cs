using BoDi;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Players.UnitTests
{
    [Binding]
    public class PlayersTestSetup : DomainTestSetupBase
    {
        // define ctor to satislfy base
        public PlayersTestSetup(IObjectContainer objectContainer) : base(objectContainer)
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
                TestTarget.Unit => new UnitPlayersTestFixture(),
                _ => throw new NotSupportedException()
            };
        }

        protected override bool RequiresAuthorizationFixture => true;

        protected override Type GetConcreteTestFixtureType => typeof(IPlayersTestFixture);

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target) =>
            new[] {
                new DomainPlayersAssemblyBootstrapper()
            };
    }
}
