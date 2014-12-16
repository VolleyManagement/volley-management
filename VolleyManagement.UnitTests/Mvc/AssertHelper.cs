using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VolleyManagement.UnitTests.Mvc
{
    public static class AssertHelper
    {
        public static void AreEqual(object obj1, object obj2, IComparer comparer)
        {
            if (comparer.Compare(obj1, obj2) != 0)
                throw new AssertFailedException();
        }
    }
}
