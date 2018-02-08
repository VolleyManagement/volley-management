namespace VolleyManagement.UnitTests.Admin.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Mvc.ViewModels;
    using UI.Areas.Admin.Models;

    /// <summary>
    /// Compares User instances
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class UserViewModelComparer : IComparer<UserViewModel>, IComparer
    {
        public int Compare(UserViewModel x, UserViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        public int Compare(object x, object y)
        {
            UserViewModel firstUser = x as UserViewModel;
            UserViewModel secondUser = y as UserViewModel;

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

        private bool AreEqual(UserViewModel x, UserViewModel y)
        {
            var playerComparer = new PlayerViewModelComparer();

            bool result = x.Id == y.Id &&
                          x.Name == y.Name &&
                          x.PersonName == y.PersonName &&
                          x.Phone == y.Phone &&
                          x.IsBlocked == y.IsBlocked;

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