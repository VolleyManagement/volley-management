namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using UI.Areas.WebApi.ViewModels.GameReports;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    internal class PivotStandingsGameViewModelComparer : IComparer, IComparer<PivotStandingsGameViewModel>, IEqualityComparer<PivotStandingsGameViewModel>
    {
        public int Compare(PivotStandingsGameViewModel x, PivotStandingsGameViewModel y)
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
            return Compare(x as PivotStandingsGameViewModel, y as PivotStandingsGameViewModel);
        }

        public bool Equals(PivotStandingsGameViewModel x, PivotStandingsGameViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(PivotStandingsGameViewModel obj)
        {
            return obj.AwayTeamId.GetHashCode();
        }

        private int CompareInternal(PivotStandingsGameViewModel x, PivotStandingsGameViewModel y)
        {
            y.AwayTeamId.Should().Be(x.AwayTeamId, $" AwayTeamId should match");
            y.HomeTeamId.Should().Be(x.HomeTeamId, $" HomeTeamId should match");

            Assert.Equal(x.Results, y.Results, new ShortGameResultViewModelComparer());

            return 0;
        }
    }
}
