using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests
{
    public interface IDomainFrameworkTestFixture : ITestFixture
    {
        void VerifyUserNotCreated();
        void VerifyUserCreated(CreateUser.Request expectedRequest);
        void MockCreateUserSuccess();
        void MockCreateUserError();
        void MockUserExists(User user);
        void MockUserNotFound();
        void MockGetUserError();
        void SetCurrentUser(User currentUser);
        User CreateAUser();
        void MockRoleStoreError();
        void SetupRole(Role role);
    }
}