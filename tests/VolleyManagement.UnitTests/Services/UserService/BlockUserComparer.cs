﻿namespace VolleyManagement.UnitTests.Services.UserService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.UsersAggregate;

    /// <summary>
    /// Comparer for user objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class BlockUserComparer : IComparer<User>, IComparer
    {
        /// <summary>
        /// Compare two user objects.
        /// </summary>
        /// <param name="x">First user to compare.</param>
        /// <param name="y">Second user to compare.</param>
        /// <returns>A signed integer that indicates
        /// the relative values of users.</returns>
        public int Compare(User x, User y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compare two user objects (non-generic implementation).
        /// </summary>
        /// <param name="x">First user to compare.</param>
        /// <param name="y">Second user to compare.</param>
        /// <returns>A signed integer that indicates
        /// the relative values of users.</returns>
        public int Compare(object x, object y)
        {
            var firstUser = x as User;
            var secondUser = y as User;

            if (firstUser == null)
            {
                return -1;
            }

            if (secondUser == null)
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
        /// <returns>True if given users have the same properties.</returns>
        private bool AreEqual(User x, User y)
        {
            return x.Id == y.Id
                && x.Email == y.Email
                && x.UserName == y.UserName
                && x.PersonName == y.PersonName
                && x.IsBlocked == y.IsBlocked;
        }
    }
}
