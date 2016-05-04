namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotStandingsEntryViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsEntryViewModelComparer : IEqualityComparer<PivotStandingsEntryViewModel>
    {
        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        public bool Equals(PivotStandingsEntryViewModel x, PivotStandingsEntryViewModel y)
        {
            return x.TeamId == y.TeamId
                && x.TeamName == y.TeamName
                && x.SetsRatio == y.SetsRatio
                && x.Points == y.Points;
        }

        /// <summary>
        /// Get objects hash code
        /// </summary>
        /// <param name="obj">object for getting hash code</param>
        /// <returns>integer hash code</returns>
        public int GetHashCode(PivotStandingsEntryViewModel obj)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(obj.TeamId);
            builder.Append(obj.TeamName);
            builder.Append(obj.SetsRatio);
            builder.Append(obj.Points);

            return builder.ToString().GetHashCode();
        }
    }
}
