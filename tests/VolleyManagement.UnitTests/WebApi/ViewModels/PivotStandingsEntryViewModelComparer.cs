namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using FluentAssertions;
    using UI.Areas.WebApi.ViewModels.GameReports;

    [ExcludeFromCodeCoverage]
    internal class PivotStandingsEntryViewModelComparer : IComparer, IComparer<PivotStandingsTeamViewModel>, IEqualityComparer<PivotStandingsTeamViewModel>
    {
        public int Compare(PivotStandingsTeamViewModel x, PivotStandingsTeamViewModel y)
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
            return Compare(x as PivotStandingsTeamViewModel, y as PivotStandingsTeamViewModel);
        }

        public bool Equals(PivotStandingsTeamViewModel x, PivotStandingsTeamViewModel y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(PivotStandingsTeamViewModel obj)
        {
            return obj.TeamId.GetHashCode();
        }

        private int CompareInternal(PivotStandingsTeamViewModel x, PivotStandingsTeamViewModel y)
        {
            var result = x.TeamName.CompareTo(y.TeamName);
            result.Should().Be(0, $"TeamName should match");

            result = x.TeamId.CompareTo(y.TeamId);
            result.Should().Be(0, $"[TeamName{x.TeamName}] TeamId should match");

            result = x.Points.CompareTo(y.Points);
            result.Should().Be(0, $"[TeamName{x.TeamName}] Points should match");

            y.SetsRatio.GetValueOrDefault().Should().BeApproximately(x.SetsRatio.GetValueOrDefault(), 0.001f, $"[TeamName{x.TeamName}] SetsRatio should match");

            return 0;
        }

    }
}
