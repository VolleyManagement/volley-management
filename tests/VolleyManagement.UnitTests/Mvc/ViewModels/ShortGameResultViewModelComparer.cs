using Xunit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;

namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    class ShortGameResultViewModelComparer : IComparer, IComparer<ShortGameResultViewModel>, IEqualityComparer<ShortGameResultViewModel>
    {
        public int Compare(ShortGameResultViewModel x, ShortGameResultViewModel y)
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
            return Compare(x as ShortGameResultViewModel, y as ShortGameResultViewModel);
        }

        public bool Equals(ShortGameResultViewModel x, ShortGameResultViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(ShortGameResultViewModel obj)
        {
            return obj.HomeSetsScore.GetHashCode();
        }

        private int CompareInternal(ShortGameResultViewModel x, ShortGameResultViewModel y)
        {
            y.HomeSetsScore.Should().Be(x.HomeSetsScore, $"HomeSetsScore should match");
            y.AwaySetsScore.Should().Be(x.AwaySetsScore, $" AwaySetsScore should match");
            y.IsTechnicalDefeat.Should().Be(x.IsTechnicalDefeat, $"IsTechnicalDefeat should match");
            return 0;
        }
    }
}
