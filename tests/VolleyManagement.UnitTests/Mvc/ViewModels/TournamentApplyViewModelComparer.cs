﻿namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentApplyViewModelComparer : IComparer<TournamentApplyViewModel>, IComparer
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(TournamentApplyViewModel x, TournamentApplyViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two tournament objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(object x, object y)
        {
            var firstTournament = x as TournamentApplyViewModel;
            var secondTournament = y as TournamentApplyViewModel;

            if (firstTournament == null)
            {
                return -1;
            }
            else if (secondTournament == null)
            {
                return 1;
            }

            return Compare(firstTournament, secondTournament);
        }

        /// <summary>
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool AreEqual(TournamentApplyViewModel x, TournamentApplyViewModel y)
        {
            var teamComparer = new TeamNameViewModelComparer();

            Assert.AreEqual(x.Id, y.Id, "Id should be equal");
            Assert.AreEqual(x.TeamId, y.TeamId, "TeamId should be equal");
            Assert.AreEqual(x.TournamentTitle, y.TournamentTitle, "TournamentTitle should be equal");

            var xTeams = x.Teams.OrderBy(t => t.Id).ToList();
            var yTeams = y.Teams.OrderBy(t => t.Id).ToList();

            Assert.AreEqual(xTeams.Count, yTeams.Count, "Number of teams in collection should be equal");

            for (var i = 0; i < xTeams.Count; i++)
            {
                Assert.IsTrue(teamComparer.AreEqual(xTeams[i], yTeams[i]), "Team at position #{i} should match.");
            }

            return true;
        }
    }
}