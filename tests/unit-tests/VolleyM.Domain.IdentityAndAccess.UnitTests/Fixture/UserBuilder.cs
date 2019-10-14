using System;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class UserBuilder
    {
        private UserId _id;
        private TenantId _tenant;
        private RoleId _role;

        private UserBuilder()
        { }

        public static UserBuilder New()
        {
            return new UserBuilder();
        }

        public User Build()
        {
            var result = new User(_id, _tenant);
            if (_role != null)
                result.AssignRole(_role);

            return result;
        }

        public UserBuilder WithId(UserId id)
            => With(() => { _id = id; });

        public UserBuilder WithTenant(TenantId tenant)
            => With(() => { _tenant = tenant; });

        public void WithRole(RoleId role)
            => With(() => { _role = role; });

        private UserBuilder With(Action action)
        {
            action();
            return this;
        }
    }
}