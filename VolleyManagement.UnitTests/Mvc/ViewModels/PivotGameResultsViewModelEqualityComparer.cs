namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotGameResultsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotGameResultsViewModelEqualityComparer : IEqualityComparer<PivotGameResultsViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public bool Equals(PivotGameResultsViewModel x, PivotGameResultsViewModel y)
        {
            return x.HomeTeamId == y.HomeTeamId
                && x.AwayTeamId == y.AwayTeamId
                && x.HomeSetsScore == y.HomeSetsScore
                && x.AwaySetsScore == y.AwaySetsScore
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.CssClass == y.CssClass;
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="PivotGameResultsViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="PivotGameResultsViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="PivotGameResultsViewModel"/>.</returns>
        public int GetHashCode(PivotGameResultsViewModel obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.HomeTeamId);
            stringBuilder.Append(obj.AwayTeamId);
            stringBuilder.Append(obj.HomeSetsScore);
            stringBuilder.Append(obj.AwaySetsScore);
            stringBuilder.Append(obj.IsTechnicalDefeat);
            stringBuilder.Append(obj.CssClass);

            return stringBuilder.ToString().GetHashCode();
        }
    }
}
