namespace VolleyManagement.UnitTests.Services.UserService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Users;

    /// <summary>
    /// Comparer for user objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserComparer : IComparer<User>, IComparer
    {
        /// <summary>
        /// Compares two user objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of users.</returns>
        public int Compare(User x, User y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares two user objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of users.</returns>
        public int Compare(object x, object y)
        {
            User firstUser = x as User;
            User secondUser = y as User;

            if (firstUser == null)
            {
                return -1;
            }
            else if (secondUser == null)
            {
                return 1;
            }

            return Compare(firstUser, secondUser);
        }

        /// <summary>
        /// Finds out whether two user objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the current users have the same properties.</returns>
        private bool IsEqual(User x, User y)
        {
            return x.Id.Equals(y.Id) &&
                x.UserName.Equals(y.UserName) &&
                x.FullName.Equals(y.FullName) &&
                x.Email.Equals(y.Email) &&
                x.Password.Equals(y.Password) &&
                x.CellPhone.Equals(y.CellPhone);
        }
    }
}
