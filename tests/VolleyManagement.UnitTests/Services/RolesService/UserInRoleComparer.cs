namespace VolleyManagement.UnitTests.Services.RolesService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.Dto;

    /// <summary>
    /// Compares Role instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserInRoleComparer : IComparer<UserInRoleDto>, IComparer
    {
        public int Compare(UserInRoleDto x, UserInRoleDto y)
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
            return Compare(x as UserInRoleDto, y as UserInRoleDto);
        }

        private int CompareInternal(UserInRoleDto x, UserInRoleDto y)
        {
            var result = y.UserId - x.UserId;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(x.UserName, y.UserName);
            if (result != 0)
            {
                return result;
            }

            result = y.RoleIds.Count - x.RoleIds.Count;
            if (result != 0)
            {
                return result;
            }

            var xEnumerator = x.RoleIds.GetEnumerator();
            var yEnumerator = x.RoleIds.GetEnumerator();


            while (xEnumerator.MoveNext() && yEnumerator.MoveNext())
            {
                result = yEnumerator.Current - xEnumerator.Current;
                if (result != 0)
                {
                    break;
                }
            }

            xEnumerator.Dispose();
            yEnumerator.Dispose();

            return result;
        }
    }
}