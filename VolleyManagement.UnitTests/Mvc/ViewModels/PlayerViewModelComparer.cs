namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Comparer for player objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerViewModelComparer : IComparer<PlayerViewModel>
    {
        /// <summary>
        /// Compares two players objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(PlayerViewModel x, PlayerViewModel y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Finds out whether two player objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given players have the same properties.</returns>
        private bool IsEqual(PlayerViewModel x, PlayerViewModel y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.BirthYear == y.BirthYear &&
                x.Height == y.Height &&
                x.Weight == y.Weight;
        }
    }
}
