namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.Division;

    /// <summary>
    /// Equality comparer for division view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class DivisionViewModelEqualityComparer
    {
        public static void AssertAreEqual(DivisionViewModel expected, DivisionViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, "Instance should not be null.");

            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix}Ids should be equal.");
            Assert.AreEqual(expected.Name, actual.Name, $"{messagePrefix}[Id:{expected.Id}]Name should be equal.");

            Assert.AreEqual(expected.Groups.Count, actual.Groups.Count, $"[Id:{expected.Id}]Number of Groups items should be equal.");
            for (var i = 0; i < expected.Groups.Count; i++)
            {
                GroupViewModelEqualityComparer.AssertAreEqual(
                    expected.Groups[i],
                    actual.Groups[i],
                    $"[Id:{expected.Id}][Group#{i}]");
            }
        }
    }
}
