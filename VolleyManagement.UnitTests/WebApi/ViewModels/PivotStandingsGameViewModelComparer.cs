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
    /// Represents an equality comparer for <see cref="PivotStandingsGameViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsGameViewModelComparer : IEqualityComparer<PivotStandingsGameViewModel>
    {
        /// <summary>
        /// Finds out whether two standings games objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given games have the same properties.</returns>
        public bool Equals(PivotStandingsGameViewModel x, PivotStandingsGameViewModel y)
        {
            return x.AwayTeamId == y.AwayTeamId
                && x.HomeTeamId == y.HomeTeamId
                && x.Results.SequenceEqual(y.Results, new ShortGameResultViewModelComparer());
        }

        /// <summary>
        /// Get objects hash code
        /// </summary>
        /// <param name="obj">object for getting hash code</param>
        /// <returns>integer hash code</returns>
        public int GetHashCode(PivotStandingsGameViewModel obj)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(obj.AwayTeamId);
            builder.Append(obj.HomeTeamId);
            builder.Append(obj.Results);

            return builder.ToString().GetHashCode();
        }
    }
}
