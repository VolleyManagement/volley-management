﻿using BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Framework.UnitTests
{
    [Binding]
    public class DomainFrameworkTestSetup : DomainTestSetupBase
    {
        public DomainFrameworkTestSetup(IObjectContainer objectContainer, FeatureContext featureContext)
	        : base(objectContainer, featureContext) { }

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
                TestTarget.Unit => new UnitDomainFrameworkTestFixture(Container),
                _ => throw new NotSupportedException()
            };
        }

        protected override bool RequiresAuthorizationFixture => false;

        protected override Type GetConcreteTestFixtureType => typeof(IDomainFrameworkTestFixture);

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target)
        {
            return Enumerable.Empty<IAssemblyBootstrapper>();
        }
    }
}