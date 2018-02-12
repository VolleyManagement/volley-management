namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal static class WeekViewModelComparer
    {
        public static void AssertAreEqual(WeekViewModel expected, WeekViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, $"{messagePrefix}Instance should not be null.");

            Assert.AreEqual(expected.Days.Count, actual.Days.Count, $"{messagePrefix}Number of Day entries does not match.");

            for (var i = 0; i < expected.Days.Count; i++)
            {
                ScheduleDayViewModelComparer.AssertAreEqual(expected.Days[i], actual.Days[i], $"{messagePrefix}[Day#{i}]");
            }
        }
    }
}