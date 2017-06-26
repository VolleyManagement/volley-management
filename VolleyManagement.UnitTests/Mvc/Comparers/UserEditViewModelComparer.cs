namespace VolleyManagement.UnitTests.Mvc.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.Users;

    /// <summary>
    /// Compares User instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserEditViewModelComparer : IComparer<UserEditViewModel>, IComparer
    {
        public int Compare(UserEditViewModel x, UserEditViewModel y)
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

            return CompareInternal(x, y);
        }

        public int Compare(object x, object y)
        {
            return Compare(x as UserEditViewModel, y as UserEditViewModel);
        }

        private int CompareInternal(UserEditViewModel x, UserEditViewModel y)
        {
            var result = y.Id - x.Id;
            if (result != 0)
            {
                return result;
            }

            if (string.CompareOrdinal(x.CellPhone, y.CellPhone) != 0
                || string.CompareOrdinal(x.FullName, y.FullName) != 0
                || string.CompareOrdinal(x.Email, y.Email) != 0)
            {
                return result;
            }

            return 0;
        }
    }
}