namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for schedule objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ScheduleViewModelComparer : IComparer<ScheduleViewModel>, IComparer
    {
        /// <summary>
        /// Compares two schedules objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of schedules.</returns>
        public int Compare(ScheduleViewModel x, ScheduleViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two schedules objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of schedules.</returns>
        public int Compare(object x, object y)
        {
            ScheduleViewModel firstSchedule = x as ScheduleViewModel;
            ScheduleViewModel secondSchedule = y as ScheduleViewModel;

            if (firstSchedule == null)
            {
                return -1;
            }
            else if (secondSchedule == null)
            {
                return 1;
            }

            return Compare(firstSchedule, secondSchedule);
        }

        /// <summary>
        /// Finds out whether two schedules objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given schedules have the same properties.</returns>
        public bool AreEqual(ScheduleViewModel x, ScheduleViewModel y)
        {
            return x.TournamentId == y.TournamentId &&
                x.TournamentName == y.TournamentName &&
                x.NumberOfRounds == y.NumberOfRounds;
        }
    }
}
