namespace VolleyManagement.UnitTests.Mvc
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections;

    public static class AssertHelper
    {
        public static void AreEqual(object obj1, object obj2, IComparer comparer)
        {
            if (comparer.Compare(obj1, obj2) != 0)
                throw new AssertFailedException();
        }
    }
}
