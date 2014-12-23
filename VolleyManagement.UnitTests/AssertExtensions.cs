namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net.Http;
    using System.Web.Mvc;

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

        /// <summary>
        /// Gets generic T model from response content
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="response">Http response message</param>
        /// <returns>T model</returns>
        public static T GetModelFromResponse<T>(HttpResponseMessage response) where T : class
        {
            ObjectContent content = response.Content as ObjectContent;
            return (T)content.Value;
        }

        /// <summary>
        /// Get generic T model by ViewResult from action view
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="result">object to convert and return</param>
        /// <returns>T result by ViewResult from action view</returns>
        public static T GetModel<T>(object result) where T : class
        {
            return (T)(result as ViewResult).ViewData.Model;
        }
    }
}
