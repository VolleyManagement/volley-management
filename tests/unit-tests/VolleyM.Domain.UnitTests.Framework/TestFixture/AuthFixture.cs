﻿using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.UnitTests.Framework
{
    internal class AuthFixture : IAuthFixture
    {
        private static readonly UserId _testUserId = new UserId("testuser@volleym.idp");
        private static readonly RoleId _testUserRoleId = new RoleId("test-user-role");

        private User _testUser;
        private Role _testUserRole;

        public void SetTestUserPermission(string context, string action)
        {
            _testUserRole.AddPermission(new Permission(context, action));
        }

        public void ConfigureTestUserRole(Container container)
        {
            var store = Substitute.For<IRolesStore>();

            _testUserRole = new Role(_testUserRoleId);
            store.Get(_testUserRoleId).Returns(_testUserRole);

            container.Register(() => store, Lifestyle.Scoped);
        }

        public void ConfigureTestUser(Container container, TenantId tenant)
        {
            _testUser = new User(_testUserId, tenant);
            _testUser.AssignRole(_testUserRoleId);

            var manager = container.GetInstance<ICurrentUserManager>();
            manager.Context = new CurrentUserContext
            {
                User = _testUser
            };
        }
    }
}