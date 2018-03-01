namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal static class DivisionTitleViewModelComparer
    {
        public static void AssertAreEqual(DivisionTitleViewModel expected, DivisionTitleViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix}Division id do not match");
            Assert.AreEqual(expected.Name, actual.Name, $"{messagePrefix}Division name do not match");

            Assert.AreEqual(expected.Rounds.Count, actual.Rounds.Count, $"{messagePrefix}Number of Round entries does not match.");

            for (var i = 0; i < expected.Rounds.Count; i++)
            {
                Assert.AreEqual(expected.Rounds[i], actual.Rounds[i], $"{messagePrefix}Round name does not match");
            }
        }
    }
}