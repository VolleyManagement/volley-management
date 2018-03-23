namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;

    [ExcludeFromCodeCoverage]
    public class DivisionViewModelComparer : IComparer, IComparer<DivisionViewModel>
    {
        public int Compare(DivisionViewModel x, DivisionViewModel y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return CompareInternal(x, y);
        }

        public int Compare(object x, object y)
        {
            return Compare(x as DivisionViewModel, y as DivisionViewModel);
        }

        public int CompareInternal(DivisionViewModel x, DivisionViewModel y)
        {
            var result = x.Id.CompareTo(y.Id);
            if (result != 0)
            {
                Assert.Fail("Ids should be equal.");
            }

            result = x.Name.CompareTo(y.Name);
            if (result != 0)
            {
                Assert.Fail($"[Id:{x.Id}]Name should be equal.");
            }

            TestHelper.AreEqual(x.Groups, y.Groups, new GroupViewModelComparer());
            return 0;
        }
    }
}
