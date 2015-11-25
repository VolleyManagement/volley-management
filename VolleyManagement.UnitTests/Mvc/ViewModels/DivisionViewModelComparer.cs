namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for division objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionViewModelComparer : IComparer<DivisionViewModel>, IComparer
    {
        /// <summary>
        /// Compares two division objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of divisions.</returns>
        public int Compare(DivisionViewModel x, DivisionViewModel y)
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
            DivisionViewModel firstDivision = x as DivisionViewModel;
            DivisionViewModel secondDivision = y as DivisionViewModel;

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
        internal bool AreEqual(DivisionViewModel x, DivisionViewModel y)
        {
            bool result = x.Id == y.Id && x.Name == y.Name && x.TournamentId == y.TournamentId;

            if (result)
            {
                var groupComparer = new GroupViewModelComparer();

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
