using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;

namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    class ShortGameResultViewModelComparer : IComparer, IComparer<ShortGameResultViewModel>
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

        private int CompareInternal(ShortGameResultViewModel x, ShortGameResultViewModel y)
        {
            Assert.AreEqual(x.HomeSetsScore, y.HomeSetsScore, $"HomeSetsScore should match");
            Assert.AreEqual(x.AwaySetsScore, y.AwaySetsScore, $" AwaySetsScore should match");
            Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, $"IsTechnicalDefeat should match");
            return 0;
        }
    }
}
