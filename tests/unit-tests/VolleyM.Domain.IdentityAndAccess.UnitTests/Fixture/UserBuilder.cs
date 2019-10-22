using System;
using Bogus;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class UserBuilder
    {
        private readonly Faker _faker = new Faker();

        private static readonly string[] Idps = new[] { "google", "facebook", "twitter", "microsoft" };

        public UserId Id { get; private set; }
        private TenantId _tenant;
        private RoleId _role;

        private UserBuilder()
        {
        }

        public static UserBuilder New()
        {
            return new UserBuilder();
        }

        public User Build()
        {
            var result = new User(Id, _tenant);
            if (_role != null)
                result.AssignRole(_role);

            return result;
        }

        /// <summary>
        /// Generates random UserId
        /// </summary>
        public UserBuilder WithAnyId()
        {
            var prefix = _faker.PickRandom(Idps);
            var suffix = _faker.Random.UShort();

            return WithId(new UserId($"{prefix}|{suffix}"));
        }

        public UserBuilder WithId(UserId id)
            => With(() => { Id = id; });

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