namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Comparer for division objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionComparer : IComparer<Division>, IComparer
    {
        /// <summary>
        /// Compares two division objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of divisions.</returns>
        public int Compare(Division x, Division y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two division objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of divisions.</returns>
        public int Compare(object x, object y)
        {
            Division firstDivision = x as Division;
            Division secondDivision = y as Division;

            if (firstDivision == null)
            {
                return -1;
            }
            else if (secondDivision == null)
            {
                return 1;
            }

            return Compare(firstDivision, secondDivision);
        }

        /// <summary>
        /// Finds out whether two division objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given divisions have the same properties.</returns>
        internal bool AreEqual(Division x, Division y)
        {
            bool result = x.Id == y.Id && x.Name == y.Name && x.TournamentId == y.TournamentId;

            if (result)
            {
                var groupComparer = new GroupComparer();

                foreach (var xGroup in x.Groups)
                {
                    bool groupFound = false;

                    foreach (var yGroup in y.Groups)
                    {
                        if (groupComparer.AreEqual(xGroup, yGroup))
                        {
                            groupFound = true;
                        }
                    }

                    if (!groupFound)
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
