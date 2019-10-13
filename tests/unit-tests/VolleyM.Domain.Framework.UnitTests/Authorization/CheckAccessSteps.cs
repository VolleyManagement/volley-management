using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.Framework.UnitTests.Authorization
{
    [FeatureFile(@"./Authorization/CheckAccess.feature")]
    public class CheckAccessSteps : DomainFrameworkStepsBase
    {
        private static readonly Dictionary<string, Permission> _permissions = new Dictionary<string, Permission>
        {
            ["Permission1"] = new Permission("ContextA", "Action1"),
            ["Permission2"] = new Permission("ContextA", "Action2")
        };

        private bool _actualResult;

        private readonly IRolesStore _rolesStore;

        public CheckAccessSteps(DomainFrameworkFixture fixture)
            : base(fixture)
        {
            _rolesStore = Substitute.For<IRolesStore>();

            Register(() => _rolesStore, Lifestyle.Scoped);
        }

        [Given(@"user has (\S+) assigned")]
        public void GivenUserHasRole(string roleKey)
        {
            var currentUser = CreateAUser();

            currentUser.AssignRole(new RoleId(roleKey));

            SetCurrentUser(currentUser);
        }

        [And(@"(\S+) has (\S+)")]
        public void AndRoleHasPermission(string roleKey, string permissionKey)
        {
            var role = new Role(new RoleId(roleKey));
            role.AddPermission(_permissions[permissionKey]);
            _rolesStore.Get(role.Id).Returns(role);
        }

        [When(@"I check access to (\S+)")]
        public void WhenCheckAccess(string permissionKey)
        {
            var authZService = Resolve<IAuthorizationService>();

            _actualResult = authZService.CheckAccess(_permissions[permissionKey]).Result;
        }

        [Then("access is granted")]
        public void ThenAccessIsGranted()
        {
            _actualResult.Should().BeTrue("because user has role with correct permission");
        }


        [Then("access is denied")]
        public void ThenAccessIsDenied()
        {
            _actualResult.Should().BeFalse("because user does not have role with correct permission");
        }

        private void SetCurrentUser(User currentUser)
        {
            var currentUserMgr = Resolve<ICurrentUserManager>();
            currentUserMgr.Context = new CurrentUserContext
            {
                User = currentUser
            };
        }

        private static User CreateAUser()
        {
            return new User(new UserId("user|abc"), TenantId.Default);
        }
    }
}