namespace VolleyManagement.UnitTests.WebApi.Standings
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.GameReports;
    using DivisionStandingsViewModel = UI.Areas.WebAPI.ViewModels.GameReports.DivisionStandingsViewModel;
    using StandingsEntryViewModel = UI.Areas.WebApi.ViewModels.GameReports.StandingsEntryViewModel;

    /// <summary>
    /// Represents <see cref="UI.Areas.Mvc.ViewModels.GameReports.StandingsViewModel"/> builder for unit tests for <see cref="GameReportsController"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionStandingsViewModelTestFixture
    {
        private List<DivisionStandingsViewModel> _divisionStandings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionStandingsViewModelTestFixture"/> class.
        /// </summary>
        public DivisionStandingsViewModelTestFixture()
        {
            _divisionStandings = new List<DivisionStandingsViewModel>
            {
                new DivisionStandingsViewModel
                    {
                        StandingsTable = new List<StandingsEntryViewModel>
                        {
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameA",
                                Position = 1,
                                Points = 5,
                                GamesTotal = 2,
                                GamesWon = 2,
                                GamesLost = 0,
                                GamesWithScoreThreeNil = 0,
                                GamesWithScoreThreeOne = 1,
                                GamesWithScoreThreeTwo = 1,
                                GamesWithScoreTwoThree = 0,
                                GamesWithScoreOneThree = 0,
                                GamesWithScoreNilThree = 0,
                                SetsWon = 6,
                                SetsLost = 3,
                                SetsRatio = 6.0f / 3,
                                BallsWon = 234,
                                BallsLost = 214,
                                BallsRatio = 234.0f / 214,
                            },
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameC",
                                Position = 2,
                                Points = 3,
                                GamesTotal = 2,
                                GamesWon = 1,
                                GamesLost = 1,
                                GamesWithScoreThreeNil = 1,
                                GamesWithScoreThreeOne = 0,
                                GamesWithScoreThreeTwo = 0,
                                GamesWithScoreTwoThree = 0,
                                GamesWithScoreOneThree = 1,
                                GamesWithScoreNilThree = 0,
                                SetsWon = 4,
                                SetsLost = 3,
                                SetsRatio = 4.0f / 3,
                                BallsWon = 166,
                                BallsLost = 105,
                                BallsRatio = 166.0f / 105,
                            },
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameB",
                                Position = 3,
                                Points = 1,
                                GamesTotal = 2,
                                GamesWon = 0,
                                GamesLost = 2,
                                GamesWithScoreThreeNil = 0,
                                GamesWithScoreThreeOne = 0,
                                GamesWithScoreThreeTwo = 0,
                                GamesWithScoreTwoThree = 1,
                                GamesWithScoreOneThree = 0,
                                GamesWithScoreNilThree = 1,
                                SetsWon = 2,
                                SetsLost = 6,
                                SetsRatio = 2.0f / 6,
                                BallsWon = 123,
                                BallsLost = 204,
                                BallsRatio = 123.0f / 204,
                            },
                        },
                        DivisionName = "DivisionNameA",
                    },
            };
        }

        public DivisionStandingsViewModelTestFixture WithMultipleDivisionsAllPossibleScores()
        {
            _divisionStandings = new List<DivisionStandingsViewModel>
            {
                    new DivisionStandingsViewModel(),
                    new DivisionStandingsViewModel(),
            };

            AddTeamsForMultipleDivisionsCase();

            return this;
        }

        private void AddTeamsForMultipleDivisionsCase()
        {
            _divisionStandings[0].StandingsTable.Add( // A
                new StandingsEntryViewModel
                {
                    Position = 1,
                    TeamName = "TeamNameA",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 363,
                    BallsLost = 355,
                    BallsRatio = (float)363 / 355
                });
            _divisionStandings[0].StandingsTable.Add( // E
                new StandingsEntryViewModel
                {
                    Position = 1,
                    TeamName = "TeamNameE",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 363,
                    BallsLost = 355,
                    BallsRatio = (float)363 / 355
                });
            _divisionStandings[0].StandingsTable.Add( // C
                new StandingsEntryViewModel
                {
                    Position = 3,
                    TeamName = "TeamNameC",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 350,
                    BallsLost = 362,
                    BallsRatio = (float)350 / 362
                });
            _divisionStandings[0].StandingsTable.Add( // D
                new StandingsEntryViewModel
                {
                    Position = 3,
                    TeamName = "TeamNameD",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 350,
                    BallsLost = 362,
                    BallsRatio = (float)350 / 362
                });
            _divisionStandings[0].StandingsTable.Add( // B
                new StandingsEntryViewModel
                {
                    Position = 5,
                    TeamName = "TeamNameB",
                    Points = 4,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 2,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 2,
                    SetsWon = 6,
                    SetsLost = 10,
                    SetsRatio = (float)6 / 10,
                    BallsWon = 349,
                    BallsLost = 345,
                    BallsRatio = (float)349 / 345
                });
            _divisionStandings[0].StandingsTable.Add( // F
                new StandingsEntryViewModel
                {
                    Position = 5,
                    TeamName = "TeamNameF",
                    Points = 4,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 2,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 2,
                    SetsWon = 6,
                    SetsLost = 10,
                    SetsRatio = (float)6 / 10,
                    BallsWon = 349,
                    BallsLost = 345,
                    BallsRatio = (float)349 / 345
                });
            _divisionStandings[0].DivisionName = "DivisionNameA";

            _divisionStandings[1].StandingsTable.Add( // G
                new StandingsEntryViewModel
                {
                    Position = 1,
                    TeamName = "TeamNameG",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 363,
                    BallsLost = 355,
                    BallsRatio = (float)363 / 355
                });
            _divisionStandings[1].StandingsTable.Add( // I
                new StandingsEntryViewModel
                {
                    Position = 2,
                    TeamName = "TeamNameI",
                    Points = 7,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 1,
                    GamesWithScoreThreeOne = 1,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 1,
                    GamesWithScoreOneThree = 1,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 9,
                    SetsLost = 7,
                    SetsRatio = (float)9 / 7,
                    BallsWon = 350,
                    BallsLost = 362,
                    BallsRatio = (float)350 / 362
                });
            _divisionStandings[1].StandingsTable.Add( // H
                new StandingsEntryViewModel
                {
                    Position = 3,
                    TeamName = "TeamNameH",
                    Points = 4,
                    GamesTotal = 4,
                    GamesWon = 2,
                    GamesLost = 2,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 2,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 2,
                    SetsWon = 6,
                    SetsLost = 10,
                    SetsRatio = (float)6 / 10,
                    BallsWon = 349,
                    BallsLost = 345,
                    BallsRatio = (float)349 / 345
                });
            _divisionStandings[1].StandingsTable.Add( // J
                new StandingsEntryViewModel
                {
                    Position = 4,
                    TeamName = "TeamNameJ",
                    Points = 0,
                    GamesTotal = 0,
                    GamesWon = 0,
                    GamesLost = 0,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 0,
                    SetsLost = 0,
                    SetsRatio = null,
                    BallsWon = 0,
                    BallsLost = 0,
                    BallsRatio = null
                });
            _divisionStandings[1].StandingsTable.Add( // K
                new StandingsEntryViewModel
                {
                    Position = 4,
                    TeamName = "TeamNameK",
                    Points = 0,
                    GamesTotal = 0,
                    GamesWon = 0,
                    GamesLost = 0,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 0,
                    SetsLost = 0,
                    SetsRatio = null,
                    BallsWon = 0,
                    BallsLost = 0,
                    BallsRatio = null
                });
            _divisionStandings[1].StandingsTable.Add( // L
                new StandingsEntryViewModel
                {
                    Position = 4,
                    TeamName = "TeamNameL",
                    Points = 0,
                    GamesTotal = 0,
                    GamesWon = 0,
                    GamesLost = 0,
                    GamesWithScoreThreeNil = 0,
                    GamesWithScoreThreeOne = 0,
                    GamesWithScoreThreeTwo = 0,
                    GamesWithScoreTwoThree = 0,
                    GamesWithScoreOneThree = 0,
                    GamesWithScoreNilThree = 0,
                    SetsWon = 0,
                    SetsLost = 0,
                    SetsRatio = null,
                    BallsWon = 0,
                    BallsLost = 0,
                    BallsRatio = null
                });
            _divisionStandings[1].DivisionName = "DivisionNameB";
        }

        public DivisionStandingsViewModelTestFixture WithLastUpdateTime(DateTime? lastUpdateTime)
        {
            // Update standings
            foreach (var standings in _divisionStandings)
            {
                standings.LastUpdateTime = lastUpdateTime;
            }

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="DivisionStandingsViewModelTestFixture"/>.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsViewModel"/>.</returns>
        public List<DivisionStandingsViewModel> Build()
        {
            return _divisionStandings;
        }
    }
}
