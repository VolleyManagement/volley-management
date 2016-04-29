namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotTableViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotTableEqualityComparer
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public static bool AreResultTablesEquals(PivotTableViewModel x, PivotTableViewModel y)
        {
            int count = (int)Math.Sqrt(x.AllGameResults.Length);
            if (x.AllGameResults.Length != y.AllGameResults.Length)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (x[i, j].Count == 0 && y[i, j].Count == 0)
                    {
                        continue;
                    }

                    if (!x[i, j].SequenceEqual(y[i, j], new PivotGameResultsViewModelEqualityComparer()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}