﻿using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.UnitTests.Authorization
{
    [Binding]
    [Scope(Feature = "Check Access")]
    public class CheckAccessSteps : DomainFrameworkStepsBase
    {
        private static readonly Dictionary<string, Permission> _permissions = new Dictionary<string, Permission>
        {
            ["Permission1"] = new Permission("ContextA", "Action1"),
            ["Permission2"] = new Permission("ContextA", "Action2")
        };

        private bool _actualResult;

        private IRolesStore _rolesStore;

        protected override void RegisterDependenciesForScenario(Container container)
        {
            base.RegisterDependenciesForScenario(container);

            _rolesStore = Substitute.For<IRolesStore>();

            IRolesStore InstanceCreator()
            {
                return _rolesStore;
            }

            container.Register(InstanceCreator, Lifestyle.Scoped);
        }

        [Given(@"user has (\S+) role assigned")]
        public void GivenUserHasRole(string roleKey)
        {
            var currentUser = CreateAUser();

            currentUser.AssignRole(new RoleId(roleKey));

            SetCurrentUser(currentUser);
        }

        [Given("role storage returns error")]
        public void GivenRoleStorageReturnsError()
        {
            var currentUser = CreateAUser();
            currentUser.AssignRole(new RoleId("test role"));
            SetCurrentUser(currentUser);

            _rolesStore.Get(Arg.Any<RoleId>()).Returns(Error.InternalError("random test error"));
        }

        [Given("user has no role")]
        public void GivenUserHasNoRole()
        {
            var currentUser = CreateAUser();
            SetCurrentUser(currentUser);
        }

        [Given(@"(\S+) has (\S+)")]
        public void GivenRoleHasPermission(string roleKey, string permissionKey)
        {
            var role = new Role(new RoleId(roleKey));
            role.AddPermission(_permissions[permissionKey]);
            _rolesStore.Get(role.Id).Returns(role);
        }

        [When(@"I check access to (\S+)")]
        public void WhenCheckAccess(string permissionKey)
        {
            var authZService = Container.GetInstance<IAuthorizationService>();

            _actualResult = authZService.CheckAccess(_permissions[permissionKey]).Result;
        }

        [When(@"I check access to permission from '(\S+)' for '(\S+)'")]
        public void WhenCheckAccess(string context, string action)
        {
            var authZService = Container.GetInstance<IAuthorizationService>();

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