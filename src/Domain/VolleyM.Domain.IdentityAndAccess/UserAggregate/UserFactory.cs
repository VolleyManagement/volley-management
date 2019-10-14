using System;

namespace VolleyM.Domain.IdentityAndAccess
{
    /// <summary>
    /// Creates user instances from persistent state
    /// </summary>
    public sealed class UserFactory
    {
        public User CreateUser(UserFactoryDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }
            return new User(userDto);
        }
    }
}