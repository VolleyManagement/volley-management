namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Generates <see cref="PivotStandingsViewModel"/> test data for unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsEntryViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of entries
        /// </summary>
        private IList<PivotStandingsTeamViewModel> _entries = new List<PivotStandingsTeamViewModel>();

        /// <summary>
        /// Adds entries to collection
        /// </summary>
        /// <returns>Builder object with collection of entries</returns>
        public PivotStandingsEntryViewModelServiceTestFixture TestEntries()
        {
            _entries.Add(
                new PivotStandingsTeamViewModel
                {
                    TeamName = "TeamNameA",
                    TeamId = 1,
                    Points = 5,
                    SetsRatio = 6.0f / 3
                });
            _entries.Add(
                 new PivotStandingsTeamViewModel
                 {
                     TeamName = "TeamNameC",
                     TeamId = 3,
                     Points = 3,
                     SetsRatio = 4.0f / 3
                 });
            _entries.Add(
                new PivotStandingsTeamViewModel
                {
                    TeamName = "TeamNameB",
                    TeamId = 2,
                    Points = 1,
                    SetsRatio = 2.0f / 6
                });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Entries collection</returns>
        public IList<PivotStandingsTeamViewModel> Build()
        {
            return _entries;
        }
    }
}
