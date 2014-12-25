namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Mvc.ViewModels.Users;

    /// <summary>
    /// Comparer for user objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserViewModelComparer : IComparer<UserViewModel>
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(UserViewModel x, UserViewModel y)
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
        /// Finds out whether two user objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given user have the same properties.</returns>
        private bool IsEqual(UserViewModel x, UserViewModel y)
        {
            return x.Id == y.Id &&
                x.UserName == y.UserName &&
                x.Password == y.Password &&
                x.FullName == y.FullName &&
                x.CellPhone == y.CellPhone &&
                x.Email == y.Email;
        }
    }
}