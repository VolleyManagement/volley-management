using System;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class UserBuilder
    {
        private UserId _id;
        private TenantId _tenant;

        private UserBuilder()
        { }

        public static UserBuilder New()
        {
            return new UserBuilder();
        }

        public User Build()
        {
            return new User(_id, _tenant);
        }

        public UserBuilder WithId(UserId id)
            => With(() => { _id = id; });

        public UserBuilder WithTenant(TenantId tenant)
            => With(() => { _tenant = tenant; });

        private UserBuilder With(Action action)
        {
            action();
            return this;
        }
    }
}