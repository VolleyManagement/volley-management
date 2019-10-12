using FluentAssertions;
using System.Collections.Generic;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using Xunit.Gherkin.Quick;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
    [FeatureFile(@"./Roles/Role.feature")]
    public class RoleSteps : Feature
    {
        private Role _role;
        private readonly IDictionary<string, Permission> _permissionStore = new Dictionary<string, Permission>
        {
            ["PermissionA"] = new Permission("ContextA", "Action1"),
            ["PermissionB"] = new Permission("ContextB", "Action2")
        };

        [Given(@"(\S+) role")]
        public void GivenRole(string roleIdString)
        {
            _role = new Role(new RoleId(roleIdString));
        }

        [When(@"I add (\S+) to it")]
        public void WhenIAddPermission(string permissionKey)
        {
            _role.AddPermission(_permissionStore[permissionKey]);
        }

        [Then(@"role has (\S+)")]
        public void ThenHasPermission(string permissionKey)
        {
            _role.HasPermission(_permissionStore[permissionKey])
                .Should().BeTrue("role has this permission assigned");
        }

        [Then(@"role does not have (\S+)")]
        public void ThenHasNoPermission(string permissionKey)
        {
            _role.HasPermission(_permissionStore[permissionKey])
                .Should().BeFalse("role does not have this permission");
        }
    }
}