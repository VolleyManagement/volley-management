namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal static class ScheduleDayViewModelComparer
    {
        public static void AssertAreEqual(ScheduleDayViewModel expected, ScheduleDayViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, $"{messagePrefix}Instance should not be null.");

            Assert.AreEqual(expected.Date, actual.Date, $"{messagePrefix}Date of game do not match");
            Assert.AreEqual(expected.Divisions.Count, actual.Divisions.Count, $"{messagePrefix}Number of Division entries does not match.");

            for (var i = 0; i < expected.Divisions.Count; i++)
            {
                DivisionTitleViewModelComparer.AssertAreEqual(expected.Divisions[i], actual.Divisions[i], $"{messagePrefix}[Division#{i}]");
            }

            Assert.AreEqual(expected.Games.Count, actual.Games.Count, $"{messagePrefix}Number of Game entries does not match.");

            for (var i = 0; i < expected.Games.Count; i++)
            {
                GameViewModelComparer.AssertAreEqual(expected.Games[i], actual.Games[i], $"{messagePrefix}[Game#{i}]");
            }
        }
    }
}