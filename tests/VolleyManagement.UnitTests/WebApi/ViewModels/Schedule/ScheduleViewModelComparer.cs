namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal static class ScheduleViewModelComparer
    {
        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="expected">The first object to compare.</param>
        /// <param name="actual">The second object to compare.</param>
        public static void AssertAreEqual(ScheduleViewModel expected, ScheduleViewModel actual)
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, "Instance should not be null.");

            Assert.AreEqual(expected.Schedule.Count, actual.Schedule.Count, $"Number of Week entries does not match.");

            for (var i = 0; i < expected.Schedule.Count; i++)
            {
                WeekViewModelComparer.AssertAreEqual(expected.Schedule[i], actual.Schedule[i], $"[Week#{i}]");
            }
        }
    }
}
