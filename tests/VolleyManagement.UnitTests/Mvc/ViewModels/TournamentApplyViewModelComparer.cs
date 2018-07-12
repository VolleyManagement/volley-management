namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;
    using UI.Areas.Mvc.ViewModels.Tournaments;
    using FluentAssertions;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentApplyViewModelComparer : IComparer<TournamentApplyViewModel>, IComparer, IEqualityComparer<TournamentApplyViewModel>
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

        public bool Equals(TournamentApplyViewModel x, TournamentApplyViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(TournamentApplyViewModel obj)
        {
            return obj.Id.GetHashCode();
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

            y.Id.Should().Be(x.Id, "Id should be equal");
            y.TeamId.Should().Be(x.TeamId, "TeamId should be equal");
            y.TournamentTitle.Should().Be(x.TournamentTitle, "TournamentTitle should be equal");

            var xTeams = x.Teams.OrderBy(t => t.Id).ToList();
            var yTeams = y.Teams.OrderBy(t => t.Id).ToList();

            yTeams.Count.Should().Be(xTeams.Count, "Number of teams in collection should be equal");

            for (var i = 0; i < xTeams.Count; i++)
            {
                Assert.True(teamComparer.AreEqual(xTeams[i], yTeams[i]), "Team at position #{i} should match.");
            }

            return true;
        }
    }
}