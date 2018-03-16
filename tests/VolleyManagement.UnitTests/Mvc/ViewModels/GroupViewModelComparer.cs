namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;


    class GroupViewModelComparer : IComparer, IComparer<GroupViewModel>
    {
        public int Compare(GroupViewModel x, GroupViewModel y)
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
            return Compare(x as GroupViewModel, y as GroupViewModel);
        }

        public int CompareInternal(GroupViewModel x, GroupViewModel y)
        {
            int result = x.Id.CompareTo(y.Id);
            if (result != 0)
            {
                Assert.Fail("Ids should be equal.");
            }

            result = x.Name.CompareTo(y.Name);
            if (result != 0)
            {
                Assert.Fail($"[Id:{x.Id}]Name should be equal.");
            }

            return 0;
        }
    }
}
