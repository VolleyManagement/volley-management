using SimpleInjector;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.UnitTests.Framework
{
    /// <summary>
    /// Common fixture to handle authentication and authorization during test
    /// </summary>
    public interface IAuthFixture
    {
        void SetTestUserPermission(string context, string action);
        void ConfigureTestUserRole(Container container);
        void ConfigureTestUser(Container container);
    }
}