using System.Threading.Tasks;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public interface IIdentityAndAccessFixture : ITestFixture
    {
        Task Initialize();

        Task ConfigureUserExists(UserId id, User user);

        Task ConfigureUserDoesNotExist(UserId id);

        Task VerifyUserCreated(User user);
    }
}