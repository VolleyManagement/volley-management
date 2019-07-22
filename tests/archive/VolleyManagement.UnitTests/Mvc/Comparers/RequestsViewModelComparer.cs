namespace VolleyManagement.UnitTests.Mvc.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Admin.Models;

    /// <summary>
    /// Compares Role instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RequestsViewModelComparer : IComparer<RequestsViewModel>, IComparer, IEqualityComparer<RequestsViewModel>
    {
        public int Compare(RequestsViewModel x, RequestsViewModel y)
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
            return Compare(x as RequestsViewModel, y as RequestsViewModel);
        }

        public bool Equals(RequestsViewModel x, RequestsViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(RequestsViewModel obj)
        {
            return obj.Id.GetHashCode();
        }

        private int CompareInternal(RequestsViewModel x, RequestsViewModel y)
        {
            var result = y.Id - x.Id;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(x.Content, y.Content);
            return result;
        }
    }
}