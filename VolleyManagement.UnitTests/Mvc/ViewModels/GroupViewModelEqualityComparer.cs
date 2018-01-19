namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.Division;

    /// <summary>
    /// Equality comparer for group view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GroupViewModelEqualityComparer
    {
        public static void AssertAreEqual(GroupViewModel expected, GroupViewModel actual, string messagePrefix = "")
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, "Instance should not be null.");

            Assert.AreEqual(expected.Id, actual.Id, $"{messagePrefix}Ids should be equal.");
            Assert.AreEqual(expected.Name, actual.Name, $"{messagePrefix}[Id:{expected.Id}]Name should be equal.");
        }
    }
}
