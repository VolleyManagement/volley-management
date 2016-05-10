namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotTeamStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotTeamStandingsViewModelEqualityComparer : IEqualityComparer<PivotTeamStandingsViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public bool Equals(PivotTeamStandingsViewModel x, PivotTeamStandingsViewModel y)
        {
            return x.TeamId == y.TeamId
                && x.TeamName == y.TeamName
                && x.Points == y.Points
                && x.SetsRatio == y.SetsRatio
                && x.Position == y.Position
                && x.BallsRatio == y.BallsRatio;
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="PivotTeamStandingsViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="PivotTeamStandingsViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="PivotTeamStandingsViewModel"/>.</returns>
        public int GetHashCode(PivotTeamStandingsViewModel obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.TeamId);
            stringBuilder.Append(obj.TeamName);
            stringBuilder.Append(obj.Points);
            stringBuilder.Append(obj.SetsRatio);
            stringBuilder.Append(obj.Position);
            stringBuilder.Append(obj.BallsRatio);

            return stringBuilder.ToString().GetHashCode();
        }
    }
}
