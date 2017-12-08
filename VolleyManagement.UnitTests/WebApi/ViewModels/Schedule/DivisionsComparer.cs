namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    [ExcludeFromCodeCoverage]
    internal class DivisionsComparer : IEqualityComparer<DivisionTitle>
    {
        public bool Equals(DivisionTitle x, DivisionTitle y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(DivisionTitle obj)
        {
            return obj.Id.GetHashCode() + obj.Name.GetHashCode() + obj.Rounds.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two standings entries objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given entries have the same properties.</returns>
        private bool AreEqual(DivisionTitle x, DivisionTitle y)
        {
            Assert.AreEqual(x.Id, y.Id, "Division id do not match");
            Assert.AreEqual(x.Name, y.Name, "Division name do not match");
            return x.Rounds.AsQueryable().SequenceEqual(y.Rounds.AsQueryable());
        }
    }
}