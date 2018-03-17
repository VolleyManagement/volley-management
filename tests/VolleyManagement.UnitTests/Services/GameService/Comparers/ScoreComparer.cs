﻿namespace VolleyManagement.UnitTests.Services.GameService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Domain.GamesAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents a comparer for <see cref="Score"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ScoreComparer : IEqualityComparer<Score>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.
        /// If both x and y are null, the method returns true.</returns>
        public bool Equals(Score x, Score y)
        {
            Assert.AreEqual(x.Home, y.Home, "Home side score should be equal");
            Assert.AreEqual(x.Away, y.Away, "Away side score should be equal");
            Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, "IsTechnicalDefeat flag should be equal");
            return true;
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="Score"/> object.
        /// </summary>
        /// <param name="obj"><see cref="Score"/> object.</param>
        /// <returns>Hash code for the specified <see cref="Score"/> object.</returns>
        public int GetHashCode(Score obj)
        {
            var builder = new StringBuilder();

            builder.Append(obj.Home);
            builder.Append(obj.Away);

            return builder.ToString().GetHashCode();
        }
    }
}
