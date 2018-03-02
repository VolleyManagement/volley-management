using System.Linq;

namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for custom asserts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TestHelper
    {
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

        public static void AreEqual<T>(List<T> expected, List<T> actual, IComparer<T> comparer)
        {
            if (expected != null || actual != null)
            {
                if (expected == null || actual == null)
                {
                    Assert.Fail("One of the colection is null");
                }

                Assert.AreEqual(expected.Count, actual.Count, "Number of items in collection should match");

                for (var i = 0; i < expected.Count; i++)
                {
                    Assert.IsTrue(
                        comparer.Compare(expected[i], actual[i]) == 0,
                        $"[Item#{i}] ");
                }
            }
        }

        public static void AreEqual<T>(ICollection<T> expected, ICollection<T> actual, IComparer<T> comparer)
        {
            if (expected != null || actual != null)
            {
                if (expected == null || actual == null)
                {
                    Assert.Fail("One of the colection is null");
                }

                Assert.AreEqual(expected.Count, actual.Count, "Number of items in collection should match");

                var expectedEnumerator = expected.GetEnumerator();
                var actualEnumerator = actual.GetEnumerator();

                while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
                {
                    Assert.IsTrue(
                           comparer.Compare(expectedEnumerator.Current, actualEnumerator.Current) == 0,
                           $"[Item#{expectedEnumerator.Current.ToString()}] ");
                }

                expectedEnumerator.Dispose();
                actualEnumerator.Dispose();

            }
        }
    }
}

