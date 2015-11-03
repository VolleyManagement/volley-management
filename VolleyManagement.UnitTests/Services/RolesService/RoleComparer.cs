namespace VolleyManagement.UnitTests.Services.RolesService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Compares Role instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleComparer : IComparer<Role>, IComparer
    {
        public int Compare(Role x, Role y)
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
            return this.Compare(x as Role, y as Role);
        }

        private int CompareInternal(Role x, Role y)
        {
            var result = y.Id - x.Id;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(x.Name, y.Name);
            return result;
        }
    }
}