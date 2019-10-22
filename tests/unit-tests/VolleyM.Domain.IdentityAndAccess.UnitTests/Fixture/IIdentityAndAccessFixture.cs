using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public interface IIdentityAndAccessFixture : ITestFixture
    {
        Task ConfigureUserExists(TenantId tenant, UserId id, User user);

        Task ConfigureUserDoesNotExist(TenantId tenant, UserId id);

        Task VerifyUserCreated(User user);
    }
}