namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Xunit;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    public class DivisionViewModelComparer : IComparer, IComparer<DivisionViewModel>, IEqualityComparer<DivisionViewModel>
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
            result.Should().Be(0, "Ids should be equal.");

            result = x.Name.CompareTo(y.Name);
            result.Should().Be(0, $"[Id:{x.Id}]Name should be equal.");

            Assert.Equal(x.Groups, y.Groups, new GroupViewModelComparer());
            return 0;
        }

        public bool Equals(DivisionViewModel x, DivisionViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(DivisionViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
