namespace VolleyManagement.UnitTests.Services.UsersService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.UsersAggregate;
    using PlayerService;

    /// <summary>
    /// Comparer for user objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
        internal class UserComparer : IComparer<User>, IComparer
        {
            /// <summary>
            /// Compare two users objects.
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
            /// Compare two users objects (non-generic implementation).
            /// </summary>
            /// <param name="x">First user to compare.</param>
            /// <param name="y">Second user to compare.</param>
            /// <returns>A signed integer that indicates
            /// the relative values of users.</returns>
            public int Compare(object x, object y)
            {
                User firstUser = x as User;
                User secondUser = y as User;

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
            var playerComparer = new PlayerComparer();
            bool result = x.Id == y.Id
                          && x.PersonName == y.PersonName
                          && x.Email == y.Email
                          && x.UserName == y.UserName
                          && x.PhoneNumber == y.PhoneNumber
                          && x.PlayerId == y.PlayerId;

            if (result && x.Player != null)
            {
                result = playerComparer.Compare(x.Player, y.Player) == 0;
            }

            if (result && x.LoginProviders != null)
            {
                foreach (var xProvider in x.LoginProviders)
                {
                    bool providerFound = false;
                    foreach (var yProvider in y.LoginProviders)
                    {
                        if (yProvider.LoginProvider == xProvider.LoginProvider
                            && xProvider.ProviderKey == yProvider.ProviderKey)
                        {
                            providerFound = true;
                        }
                    }

                    if (!providerFound)
                    {
                        result = false;
                        break;
                    }
                }
            }

            if (result && x.Roles != null)
            {
                foreach (var xRole in x.Roles)
                {
                    bool roleFound = false;
                    foreach (var yRole in y.Roles)
                    {
                        if (yRole.Id == xRole.Id
                            && xRole.Name == yRole.Name)
                        {
                            roleFound = true;
                        }
                    }

                    if (!roleFound)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
