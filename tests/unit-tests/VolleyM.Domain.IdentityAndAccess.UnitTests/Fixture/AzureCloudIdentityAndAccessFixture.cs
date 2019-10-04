using FluentAssertions;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class AzureCloudIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private readonly DomainPipelineFixtureBase _baseFixture;

        public AzureCloudIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public void Initialize()
        {
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
        }

        public void VerifyUserCreated(User user)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();

            var savedUser = repo.Get(user.Tenant, user.Id).Result;

            savedUser.Should().BeSuccessful("user should be created");
            savedUser.Value.Should().BeEquivalentTo(user, "all attributes should be saved correctly");
        }
    }
}