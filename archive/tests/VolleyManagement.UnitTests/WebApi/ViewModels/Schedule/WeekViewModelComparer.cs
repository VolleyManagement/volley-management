namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using UI.Areas.WebAPI.ViewModels.Schedule;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    internal static class WeekViewModelComparer
    {
        public static void AssertAreEqual(WeekViewModel expected, WeekViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.False(expected == null || actual == null, $"{messagePrefix}Instance should not be null.");

            actual.Days.Count.Should().Be(expected.Days.Count, $"{messagePrefix}Number of Day entries does not match.");

            for (var i = 0; i < expected.Days.Count; i++)
            {
                ScheduleDayViewModelComparer.AssertAreEqual(expected.Days[i], actual.Days[i], $"{messagePrefix}[Day#{i}]");
            }
        }
    }
}