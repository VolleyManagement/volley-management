using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Framework.UnitTests
{
    public class DomainFrameworkStepsBase : SpecFlowBindingBase
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

        protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers()
        {
            return Enumerable.Empty<IAssemblyBootstrapper>();
        }
    }
}