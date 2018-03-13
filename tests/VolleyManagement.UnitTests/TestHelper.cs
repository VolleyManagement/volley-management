namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    /// <summary>
    /// Class for custom asserts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TestHelper
    {
        private const string COLLECTION_IS_NULL_MESSAGE = "One of the collections is null.";
        private const string COLLECTIONS_COUNT_UNEQUAL_MESSAGE = "Number of items in collections should match.";

       /// <summary>
        /// Test equals of two objects with specific comparer.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="expected">Expected result</param>
        /// <param name="actual">Actual result</param>
        /// <param name="comparer">Specific comparer</param>
        public static void AreEqual<T>(T expected, T actual, IComparer<T> comparer)
        {
            int equalsResult = 0;
            int compareResult = comparer.Compare(expected, actual);

            Assert.AreEqual(equalsResult, compareResult);
        }

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) =>
            AreEqual(expected, actual, null, string.Empty);

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IComparer<T> comparer) =>
            AreEqual(expected, actual, comparer, string.Empty);

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message) =>
            AreEqual(expected, actual, null, message);

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IComparer<T> comparer, string message)
        {
            if (expected == null || actual == null)
            {
                Assert.Fail(COLLECTION_IS_NULL_MESSAGE);
            }

            Assert.AreEqual(expected.Count(), actual.Count(), COLLECTIONS_COUNT_UNEQUAL_MESSAGE);

            string preparedErrorMessage;
            foreach (var pair in expected.Zip(actual, (e, a) => new { Expected = e, Actual = a }))
            {
                preparedErrorMessage = !string.IsNullOrEmpty(message) ? message
                        : $"[Item#{pair.Expected.ToString()}] ";

                if (comparer == null)
                {
                    Assert.AreEqual(pair.Expected,
                        pair.Actual,
                        preparedErrorMessage);
                }
                else
                {
                    Assert.IsTrue(
                        comparer.Compare(pair.Expected, pair.Actual) == 0,
                        preparedErrorMessage);
                }
            }
        }
    }
}
