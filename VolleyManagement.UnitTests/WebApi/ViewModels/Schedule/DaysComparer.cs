namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal class DaysComparer : IEqualityComparer<ScheduleDayViewModel>
    {
        public bool Equals(ScheduleDayViewModel x, ScheduleDayViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(ScheduleDayViewModel obj)
        {
            return obj.Date.GetHashCode() + obj.Games.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(ScheduleDayViewModel x, ScheduleDayViewModel y)
        {
            Assert.AreEqual(x.Date, y.Date, "Date of game do not match");
            return x.Divisions.AsQueryable().SequenceEqual(y.Divisions.AsQueryable(), new DivisionsComparer()) &&
            x.Games.AsQueryable().SequenceEqual(y.Games.AsQueryable(), new GameViewModelComparer());
        }
    }
}