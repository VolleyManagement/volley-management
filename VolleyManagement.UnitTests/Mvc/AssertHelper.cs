namespace VolleyManagement.UnitTests.Mvc
{
    using System.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Assert helper class
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="obj1">first object</param>
        /// <param name="obj2">second object</param>
        /// <param name="comparer">object comparer</param>
        public static void AreEqual(object obj1, object obj2, IComparer comparer)
        {
            if (comparer.Compare(obj1, obj2) != 0)
            {
                throw new AssertFailedException();
            }
        }
    }
}
