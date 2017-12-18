namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    internal class WeekViewModelComparer : IEqualityComparer<WeekViewModel>
    {
        public bool Equals(WeekViewModel x, WeekViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(WeekViewModel obj)
        {
            return obj.Days.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(WeekViewModel x, WeekViewModel y)
        {
            return x.Days.AsQueryable().SequenceEqual(y.Days.AsQueryable(), new ScheduleDayViewModelComparer());
        }
    }
}