using VolleyManagement.Contracts.Authorization;

namespace VolleyManagement.Specs.Infrastructure
{
    /// <summary>
    /// Current user service for ITs
    /// </summary>
    public class MockCurrentUserService : ICurrentUserService
    {
        private int _userId = 1;//Admin

        public int GetCurrentUserId()
        {
            return _userId;
        }

        public void SetCurrentUser(int id)
        {
            _userId = id;
        }
    }
}