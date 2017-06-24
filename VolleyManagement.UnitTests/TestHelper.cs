namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    //using Ninject;

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

        /// <summary>
        /// Creates default mock and registers object in the container
        /// </summary>
        /// <typeparam name="T">Mocked service type</typeparam>
        /// <param name="kernel">Ninject kernel instance</param>
        /// <param name="instance">Mock instance</param>
        /*public static void RegisterDefaultMock<T>(this IKernel kernel, out Mock<T> instance) where T : class
        {
            instance = new Mock<T>();
            kernel.Bind<T>().ToConstant(instance.Object);
        }*/
    }
}
