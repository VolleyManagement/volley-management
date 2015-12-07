namespace VolleyManagement.UnitTests.Mvc.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;

    /// <summary>
    /// Compares User instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserViewModelComparer : IComparer<UserViewModel>, IComparer
    {
        public int Compare(UserViewModel x, UserViewModel y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return this.CompareInternal(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as UserViewModel, y as UserViewModel);
        }

        private int CompareInternal(UserViewModel x, UserViewModel y)
        {
            var result = y.Id - x.Id;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(x.UserName, y.UserName);
            return result;
        }
    }
}