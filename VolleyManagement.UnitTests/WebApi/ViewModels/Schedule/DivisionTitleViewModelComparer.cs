namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal class DivisionTitleViewModelComparer : IEqualityComparer<DivisionTitleViewModel>
    {
        public bool Equals(DivisionTitleViewModel x, DivisionTitleViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(DivisionTitleViewModel obj)
        {
            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(DivisionTitleViewModel x, DivisionTitleViewModel y)
        {
            Assert.AreEqual(x.Id, y.Id, "Division id do not match");
            Assert.AreEqual(x.Name, y.Name, "Division name do not match");
            return x.Rounds.AsQueryable().SequenceEqual(y.Rounds.AsQueryable());
        }
    }
}