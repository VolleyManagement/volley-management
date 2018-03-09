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

            var expectedGroupsEnumerator = expected.Groups.GetEnumerator();
            var actualGroupsEnumerator = actual.Groups.GetEnumerator();

            while (expectedGroupsEnumerator.MoveNext() && actualGroupsEnumerator.MoveNext())
            {
                GroupViewModelEqualityComparer.AssertAreEqual(expectedGroupsEnumerator.Current,
                    actualGroupsEnumerator.Current,
                    $"[Id:{expectedGroupsEnumerator.Current.Id}][Group#{expectedGroupsEnumerator.Current.ToDomain().Id}]");
            }

            expectedGroupsEnumerator.Dispose();
            actualGroupsEnumerator.Dispose();
        }
    }
}
