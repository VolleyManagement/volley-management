﻿using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors.UnitTests
{
    public class ContributorsStepsBase : SpecFlowBindingBase
    {
        [BeforeTestRun(Order = ONETIME_DOMAIN_FIXTURE_ORDER)]
        public static void OneTimeSetup()
        {
            BeforeTestRun();
        }

        [AfterTestRun]
        public static void OneTimeTearDown()
        {
            AfterTestRun();
        }

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers() =>
            new[] {
                new DomainContributorsAssemblyBootstrapper()
            };
    }
}