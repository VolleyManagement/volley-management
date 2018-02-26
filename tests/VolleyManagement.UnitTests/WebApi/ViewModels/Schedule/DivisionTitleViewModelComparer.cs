namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal static class DivisionTitleViewModelComparer
    {
        public static void AssertAreEqual(DivisionTitleViewModel expected, DivisionTitleViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix}Division id do not match");
            Assert.AreEqual(expected.Name, actual.Name, $"{messagePrefix}Division name do not match");
            CollectionAssert.AreEqual(expected.Rounds, actual.Rounds, $"{messagePrefix}Division Rounds collection do not match");
        }
    }
}