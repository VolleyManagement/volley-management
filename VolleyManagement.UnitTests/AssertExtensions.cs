namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for custom asserts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class AssertExtensions
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
    }
}