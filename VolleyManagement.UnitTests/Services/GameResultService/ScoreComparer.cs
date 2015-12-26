namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents a comparer for <see cref="Score"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ScoreComparer : IComparer<Score>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="Score"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="Score"/> x and y.</returns>
        public int Compare(Score x, Score y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="Score"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="Score"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            Score firstScore = x as Score;
            Score secondScore = y as Score;

            if (firstScore == null)
            {
                return -1;
            }
            else if (secondScore == null)
            {
                return 1;
            }

            return Compare(firstScore, secondScore);
        }

        /// <summary>
        /// Finds out whether two <see cref="Score"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="Score"/> objects are equal.</returns>
        internal bool AreEqual(Score x, Score y)
        {
            return x.Home == y.Home && x.Away == y.Away;
        }
    }
}
