using System.Threading.Tasks;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public interface IIdentityAndAccessFixture : ITestFixture
    {
        void Initialize();

        void ConfigureUserExists(UserId id, User user);

        void ConfigureUserDoesNotExist(UserId id);

        void VerifyUserCreated(User user);
    }
}