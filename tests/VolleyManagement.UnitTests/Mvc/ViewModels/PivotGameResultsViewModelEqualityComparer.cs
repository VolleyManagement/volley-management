namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotGameResultViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotGameResultsViewModelEqualityComparer : IEqualityComparer<PivotGameResultViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public bool Equals(PivotGameResultViewModel x, PivotGameResultViewModel y)
        {
            return AreEqual(x, y);
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="PivotGameResultViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="PivotGameResultViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="PivotGameResultViewModel"/>.</returns>
        public int GetHashCode(PivotGameResultViewModel obj)
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

        public static bool AreEqual(PivotGameResultViewModel expected, PivotGameResultViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.HomeTeamId, actual.HomeTeamId, $"{messagePrefix} HomeTeamId should match.");
            Assert.AreEqual(expected.AwayTeamId, actual.AwayTeamId, $"{messagePrefix} AwayTeamId should match.");
            Assert.AreEqual(expected.HomeSetsScore, actual.HomeSetsScore, $"{messagePrefix} HomeSetsScore should match.");
            Assert.AreEqual(expected.AwaySetsScore, actual.AwaySetsScore, $"{messagePrefix} AwaySetsScore should match.");
            Assert.AreEqual(expected.IsTechnicalDefeat, actual.IsTechnicalDefeat, $"{messagePrefix} IsTechnicalDefeat should match.");
            Assert.AreEqual(expected.CssClass, actual.CssClass, $"{messagePrefix} CssClass should match.");
            return true;
        }
    }
}
