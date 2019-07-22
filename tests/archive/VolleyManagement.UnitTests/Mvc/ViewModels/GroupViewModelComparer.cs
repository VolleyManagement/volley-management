namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Xunit;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;
    using FluentAssertions;


    class GroupViewModelComparer : IComparer, IComparer<GroupViewModel>, IEqualityComparer<GroupViewModel>
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
            var result = x.Id.CompareTo(y.Id);
            result.Should().Be(0, "Ids should be equal.");

            result = x.Name.CompareTo(y.Name);
            result.Should().Be(0, $"[Id:{x.Id}]Name should be equal.");

            return 0;
        }

        public bool Equals(GroupViewModel x, GroupViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(GroupViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
