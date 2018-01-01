namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    internal class ScheduleViewModelComparer : IComparer<ScheduleViewModel>, IComparer
    {
        public int Compare(ScheduleViewModel x, ScheduleViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        public int Compare(object x, object y)
        {
            var firstGame = x as ScheduleViewModel;
            var secondGame = y as ScheduleViewModel;

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
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(ScheduleViewModel x, ScheduleViewModel y)
        {
            return x.Schedule.AsQueryable().SequenceEqual(y.Schedule.AsQueryable(), new WeekViewModelComparer());
        }
    }
}
