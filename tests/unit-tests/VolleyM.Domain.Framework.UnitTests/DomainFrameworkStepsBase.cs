using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess;
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

        /// <summary>
        /// Mocked handlers does not work with Authorization decorator
        /// This methods mocks internal cache to avoid attribute resolution based on required structure
        /// </summary>
        protected void SetupPermissionAttribute(Type requestType, PermissionAttribute attribute)
        {
            var permissionAttributeMap = Container.GetInstance<PermissionAttributeMappingStore>();
            permissionAttributeMap.AddOrUpdate(
                requestType,
                attribute,
                (_, existing) => existing);
        }

        protected void SetCurrentUser(User currentUser)
        {
            var currentUserMgr = Container.GetInstance<ICurrentUserManager>();
            currentUserMgr.Context = new CurrentUserContext
            {
                User = currentUser
            };
        }

        protected static User CreateAUser()
        {
            return new User(new UserId("user|abc"), TenantId.Default);
        }
    }
}