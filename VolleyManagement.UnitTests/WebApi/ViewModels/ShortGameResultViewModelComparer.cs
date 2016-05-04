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
    /// Represents an equality comparer for <see cref="ShortGameResultViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ShortGameResultViewModelComparer : IEqualityComparer<ShortGameResultViewModel>
    {
        /// <summary>
        /// Finds out whether two games objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given games have the same properties.</returns>
        public bool Equals(ShortGameResultViewModel x, ShortGameResultViewModel y)
        {
            return x.HomeSetsScore == y.HomeSetsScore
                && x.AwaySetsScore == y.AwaySetsScore
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat;
        }

        /// <summary>
        /// Get objects hash code
        /// </summary>
        /// <param name="obj">object for getting hash code</param>
        /// <returns>integer hash code</returns>
        public int GetHashCode(ShortGameResultViewModel obj)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(obj.AwaySetsScore);
            builder.Append(obj.HomeSetsScore);
            builder.Append(obj.IsTechnicalDefeat);

            return builder.ToString().GetHashCode();
        }
    }
}
