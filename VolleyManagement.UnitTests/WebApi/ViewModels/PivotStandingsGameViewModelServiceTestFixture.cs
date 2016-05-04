namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Generates <see cref="PivotStandingsGameViewModel"/> test data for unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotStandingsGameViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of entries
        /// </summary>
        private IList<PivotStandingsGameViewModel> _games = new List<PivotStandingsGameViewModel>();

        /// <summary>
        /// Adds entries to collection
        /// </summary>
        /// <returns>Builder object with collection of entries</returns>
        public PivotStandingsGameViewModelServiceTestFixture TestEntries()
        {
            var firstResultCollection = new List<ShortGameResultViewModel>();
            firstResultCollection.Add(
                new ShortGameResultViewModel
                {
                    HomeSetsScore = 3,
                    AwaySetsScore = 2,
                    IsTechnicalDefeat = false
                });
            var secondResultCollection = new List<ShortGameResultViewModel>();
            secondResultCollection.Add(
              new ShortGameResultViewModel
              {
                  HomeSetsScore = 3,
                  AwaySetsScore = 0,
                  IsTechnicalDefeat = true
              });
            var thirdResultCollection = new List<ShortGameResultViewModel>();
            thirdResultCollection.Add(
              new ShortGameResultViewModel
              {
                  HomeSetsScore = 1,
                  AwaySetsScore = 3,
                  IsTechnicalDefeat = false
              });

            _games.Add(
                new PivotStandingsGameViewModel
                {
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    Results = firstResultCollection
                });
            _games.Add(
                 new PivotStandingsGameViewModel
                 {
                     HomeTeamId = 3,
                     AwayTeamId = 1,
                     Results = secondResultCollection
                 });
            _games.Add(
                new PivotStandingsGameViewModel
                {
                    HomeTeamId = 2,
                    AwayTeamId = 1,
                    Results = thirdResultCollection
                });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Entries collection</returns>
        public IList<PivotStandingsGameViewModel> Build()
        {
            return _games;
        }
    }
}
