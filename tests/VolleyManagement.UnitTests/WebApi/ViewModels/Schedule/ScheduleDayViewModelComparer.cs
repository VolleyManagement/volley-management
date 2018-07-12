namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using UI.Areas.WebAPI.ViewModels.Schedule;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    internal static class ScheduleDayViewModelComparer
    {
        public static void AssertAreEqual(ScheduleDayViewModel expected, ScheduleDayViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.False(expected == null || actual == null, $"{messagePrefix}Instance should not be null.");

            actual.Date.Should().Be(expected.Date, $"{messagePrefix}Date of game do not match");
            actual.Divisions.Count.Should().Be(expected.Divisions.Count, $"{messagePrefix}Number of Division entries does not match.");

            for (var i = 0; i < expected.Divisions.Count; i++)
            {
                DivisionTitleViewModelComparer.AssertAreEqual(expected.Divisions[i], actual.Divisions[i], $"{messagePrefix}[Division#{i}]");
            }

            actual.Games.Count.Should().Be(expected.Games.Count, $"{messagePrefix}Number of Game entries does not match.");

            for (var i = 0; i < expected.Games.Count; i++)
            {
                GameViewModelComparer.AssertAreEqual(expected.Games[i], actual.Games[i], $"{messagePrefix}[Game#{i}]");
            }
        }
    }
}