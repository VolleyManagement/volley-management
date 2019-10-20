using FluentAssertions;
using SimpleInjector;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.UnitTests.Authorization
{
    [Binding]
    [Scope(Feature = "Check Access")]
    public class CheckAccessSteps
    {
        private static readonly Dictionary<string, Permission> _permissions = new Dictionary<string, Permission>
        {
            ["Permission1"] = new Permission("ContextA", "Action1"),
            ["Permission2"] = new Permission("ContextA", "Action2")
        };

        private readonly IDomainFrameworkTestFixture _testFixture;
        private readonly Container _container;

        private bool _actualResult;

        public CheckAccessSteps(Container container, IDomainFrameworkTestFixture testFixture)
        {
            _container = container;
            _testFixture = testFixture;
        }

        [Given(@"user has (\S+) role assigned")]
        public void GivenUserHasRole(string roleKey)
        {
            var currentUser = _testFixture.CreateAUser();

            currentUser.AssignRole(new RoleId(roleKey));

            _testFixture.SetCurrentUser(currentUser);
        }

        [Given("role storage returns error")]
        public void GivenRoleStorageReturnsError()
        {
            var currentUser = _testFixture.CreateAUser();
            currentUser.AssignRole(new RoleId("test role"));
            _testFixture.SetCurrentUser(currentUser);

            _testFixture.MockRoleStoreError();
        }

        [Given("user has no role")]
        public void GivenUserHasNoRole()
        {
            var currentUser = _testFixture.CreateAUser();
            _testFixture.SetCurrentUser(currentUser);
        }

        [Given(@"(\S+) has (\S+)")]
        public void GivenRoleHasPermission(string roleKey, string permissionKey)
        {
            var role = new Role(new RoleId(roleKey));
            role.AddPermission(_permissions[permissionKey]);
            
            _testFixture.SetupRole(role);
        }

        [When(@"I check access to (\S+)")]
        public void WhenCheckAccess(string permissionKey)
        {
            var authZService = _container.GetInstance<IAuthorizationService>();

            _actualResult = authZService.CheckAccess(_permissions[permissionKey]).Result;
        }

        [When(@"I check access to permission from '(\S+)' for '(\S+)'")]
        public void WhenCheckAccess(string context, string action)
        {
            var authZService = _container.GetInstance<IAuthorizationService>();

            _actualResult = authZService.CheckAccess(new Permission(context, action)).Result;
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
    }
}