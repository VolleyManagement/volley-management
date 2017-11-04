namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.WebApi.ViewModels.Games;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ScheduleByRoundViewModelComparer : IComparer<ScheduleByRoundViewModel>, IComparer
    {
        /// <summary>
        /// Compares two game objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(ScheduleByRoundViewModel x, ScheduleByRoundViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two game objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(object x, object y)
        {
            ScheduleByRoundViewModel firstGame = x as ScheduleByRoundViewModel;
            ScheduleByRoundViewModel secondGame = y as ScheduleByRoundViewModel;

            if (firstGame == null)
            {
                return -1;
            }
            else if (secondGame == null)
            {
                return 1;
            }

            return Compare(firstGame, secondGame);
        }

        /// <summary>
        /// Finds out whether two game objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given games have the same properties.</returns>
        private bool AreEqual(ScheduleByRoundViewModel x, ScheduleByRoundViewModel y)
        {
            return x.Round == y.Round &&
            x.GameResults.AsQueryable<GameViewModel>()
            .SequenceEqual(y.GameResults.AsQueryable<GameViewModel>(), new GameViewModelComparer());
        }
    }
}
