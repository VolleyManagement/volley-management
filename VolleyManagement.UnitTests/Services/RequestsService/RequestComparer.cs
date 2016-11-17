namespace VolleyManagement.UnitTests.Services.RequestsService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.RequestsAggregate;

    /// <summary>
    /// Comparer for request objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class RequestComparer : IComparer<Request>, IComparer
    {
        /// <summary>
        /// Compares two request objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of requests.</returns>
        public int Compare(Request x, Request y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two request objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of requests.</returns>
        public int Compare(object x, object y)
        {
            Request firstRequest = x as Request;
            Request secondRequest = y as Request;

            if (firstRequest == null)
            {
                return -1;
            }
            else if (secondRequest == null)
            {
                return 1;
            }

            return Compare(firstRequest, secondRequest);
        }

        /// <summary>
        /// Finds out whether two request objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given requests have the same properties.</returns>
        public bool AreEqual(Request x, Request y)
        {
            return x.Id == y.Id &&
                   x.UserId == y.UserId &&
                   x.PlayerId == y.PlayerId;
        }
    }
}