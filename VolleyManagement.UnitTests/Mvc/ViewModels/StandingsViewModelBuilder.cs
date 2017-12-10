namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents <see cref="StandingsViewModel"/> builder for unit tests for <see cref="GameReportsController"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsViewModelBuilder
    {
        private StandingsViewModel _standingsViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandingsViewModelBuilder"/> class.
        /// </summary>
        public StandingsViewModelBuilder()
        {
            _standingsViewModel = new StandingsViewModel
            {
                TournamentId = 1,
                TournamentName = "Name",
                StandingsTable = new List<DivisionStandingsViewModel>()
                {
                    new DivisionStandingsViewModel
                    {
                        StandingsEntries = new List<StandingsEntryViewModel>
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
                                BallsRatio = 234.0f / 214
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
                                BallsRatio = 166.0f / 105
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
                                BallsRatio = 123.0f / 204
                            }
                        }
                    }
                },

                PivotTable = new List<PivotTableViewModel>()
                {
                    new PivotTableViewModel
                    {
                        TeamsStandings = GetPivotTeamsStandings(),
                        AllGameResults = GetPivotTable()
                    }
                }
            };
        }

        public StandingsViewModelBuilder WithMultipleDivisionsAllPossibleScores()
        {
            _standingsViewModel = new StandingsViewModel
            {
                TournamentId = 1,
                TournamentName = "Name",
                StandingsTable = new List<DivisionStandingsViewModel>
                {
                    new DivisionStandingsViewModel(),
                    new DivisionStandingsViewModel(),
                },
                PivotTable = new List<PivotTableViewModel>
                {
                    new PivotTableViewModel(),
                    new PivotTableViewModel()
                }
            };

            AddTeamsForMultipleDivisionsCase();

            AddPivotTeamsForMultipleDivisionsCase();

            AddFirstPivotResultsForMultipleDivisionsCase();
            AddSecondPivotResultsForMultipleDivisionsCase();

            return this;
        }

        private void AddTeamsForMultipleDivisionsCase()
        {
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // A
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
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // E
                new StandingsEntryViewModel
                {
                    Position = 2,
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
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // C
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
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // D
                new StandingsEntryViewModel
                {
                    Position = 4,
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
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // B
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
            _standingsViewModel.StandingsTable[0].StandingsEntries.Add( // F
                new StandingsEntryViewModel
                {
                    Position = 6,
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // G
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // I
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // H
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // J
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // K
                new StandingsEntryViewModel
                {
                    Position = 5,
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
            _standingsViewModel.StandingsTable[1].StandingsEntries.Add( // L
                new StandingsEntryViewModel
                {
                    Position = 6,
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
        }

        private void AddFirstPivotResultsForMultipleDivisionsCase()
        {
            const int teamsCount = 6;
            var table = new List<PivotGameResultViewModel>[teamsCount * teamsCount];
            _standingsViewModel.PivotTable[0].AllGameResults = table;
            for (int i = 0; i < teamsCount; i++)
            {
                for (var j = 0; j < teamsCount; j++)
                {
                    table[i * teamsCount + j] = new List<PivotGameResultViewModel>();
                    if (i == j)
                    {
                        table[i * teamsCount + j].Add(PivotGameResultViewModel.GetNonPlayableCell());
                    }
                }
            }

            // Group 1
            table[0 * teamsCount + 4].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });
            table[0 * teamsCount + 4].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });

            table[0 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });
            table[0 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            table[4 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });
            table[4 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });

            table[4 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });
            table[4 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });

            table[2 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });
            table[2 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[2 * teamsCount + 4].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });
            table[2 * teamsCount + 4].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });

            // Group 2
            table[3 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 5,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });
            table[3 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 5,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[3 * teamsCount + 5].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 6,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });
            table[3 * teamsCount + 5].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 6,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });

            table[1 * teamsCount + 3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 5,
                AwayTeamId = 4,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });
            table[1 * teamsCount + 3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 5,
                AwayTeamId = 4,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            table[1 * teamsCount + 5].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 5,
                AwayTeamId = 6,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });
            table[1 * teamsCount + 5].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 5,
                AwayTeamId = 6,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });

            table[5 * teamsCount + 3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 6,
                AwayTeamId = 4,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });
            table[5 * teamsCount + 3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 6,
                AwayTeamId = 4,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });

            table[5 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 6,
                AwayTeamId = 5,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });
            table[5 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 6,
                AwayTeamId = 5,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });
        }

        private void AddSecondPivotResultsForMultipleDivisionsCase()
        {
            const int teamsCount = 6;
            var table = new List<PivotGameResultViewModel>[teamsCount * teamsCount];
            _standingsViewModel.PivotTable[1].AllGameResults = table;
            for (int i = 0; i < teamsCount; i++)
            {
                for (var j = 0; j < teamsCount; j++)
                {
                    table[i * teamsCount + j] = new List<PivotGameResultViewModel>();
                    if (i == j)
                    {
                        table[i * teamsCount + j].Add(PivotGameResultViewModel.GetNonPlayableCell());
                    }
                }
            }

            table[0 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 9,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });
            table[0 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 9,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            table[0 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 8,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });
            table[0 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 8,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });

            table[1 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 9,
                AwayTeamId = 7,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });
            table[1 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 9,
                AwayTeamId = 7,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[1 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 9,
                AwayTeamId = 8,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });
            table[1 * teamsCount + 2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 9,
                AwayTeamId = 8,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_0
            });

            table[2 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 8,
                AwayTeamId = 7,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });
            table[2 * teamsCount + 0].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 8,
                AwayTeamId = 7,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });

            table[2 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 8,
                AwayTeamId = 9,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });
            table[2 * teamsCount + 1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 8,
                AwayTeamId = 9,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_0_3
            });
        }

        private void AddPivotTeamsForMultipleDivisionsCase()
        {
            _standingsViewModel.PivotTable[0].TeamsStandings = new List<PivotTeamStandingsViewModel>();
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // A
                new PivotTeamStandingsViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Position = 1,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                });
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // E
                new PivotTeamStandingsViewModel
                {
                    TeamId = 5,
                    TeamName = "TeamNameE",
                    Position = 2,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                });
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // C
                new PivotTeamStandingsViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Position = 3,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                });
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // D
                new PivotTeamStandingsViewModel
                {
                    TeamId = 4,
                    TeamName = "TeamNameD",
                    Position = 4,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                });
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // B
                new PivotTeamStandingsViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Position = 5,
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                });
            _standingsViewModel.PivotTable[0].TeamsStandings.Add( // F
                new PivotTeamStandingsViewModel
                {
                    TeamId = 6,
                    TeamName = "TeamNameF",
                    Position = 6,
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                });

            _standingsViewModel.PivotTable[1].TeamsStandings = new List<PivotTeamStandingsViewModel>();
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // G
                new PivotTeamStandingsViewModel
                {
                    TeamId = 7,
                    TeamName = "TeamNameG",
                    Position = 1,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)363 / 355
                });
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // I
                new PivotTeamStandingsViewModel
                {
                    TeamId = 9,
                    TeamName = "TeamNameI",
                    Position = 2,
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                    BallsRatio = (float)350 / 362
                });
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // H
                new PivotTeamStandingsViewModel
                {
                    TeamId = 8,
                    TeamName = "TeamNameH",
                    Position = 3,
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                    BallsRatio = (float)349 / 345
                });
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // J
                new PivotTeamStandingsViewModel
                {
                    TeamId = 10,
                    TeamName = "TeamNameJ",
                    Position = 4,
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                });
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // K
                new PivotTeamStandingsViewModel
                {
                    TeamId = 11,
                    TeamName = "TeamNameK",
                    Position = 5,
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                });
            _standingsViewModel.PivotTable[1].TeamsStandings.Add( // L
                new PivotTeamStandingsViewModel
                {
                    TeamId = 12,
                    TeamName = "TeamNameL",
                    Position = 6,
                    Points = 0,
                    SetsRatio = null,
                    BallsRatio = null
                });

        }

        /// <summary>
        /// Sets the tournament's data with 2 teams scores completely equal.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithTwoTeamsScoresCompletelyEqual()
        {
            _standingsViewModel = new StandingsViewModel
            {
                TournamentId = 1,
                TournamentName = "Name",
                StandingsTable = new List<DivisionStandingsViewModel>()
                {
                    new DivisionStandingsViewModel
                    {
                        StandingsEntries = new List<StandingsEntryViewModel>()
                        {
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameB",
                                Position = 1,
                                Points = 3,
                                GamesTotal = 1,
                                GamesWon = 1,
                                GamesLost = 0,
                                GamesWithScoreThreeNil = 0,
                                GamesWithScoreThreeOne = 1,
                                GamesWithScoreThreeTwo = 0,
                                GamesWithScoreTwoThree = 0,
                                GamesWithScoreOneThree = 0,
                                GamesWithScoreNilThree = 0,
                                SetsWon = 3,
                                SetsLost = 1,
                                SetsRatio = 3.0f / 1,
                                BallsWon = 102,
                                BallsLost = 96,
                                BallsRatio = 102.0f / 96
                            },
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameC",
                                Position = 1,
                                Points = 3,
                                GamesTotal = 1,
                                GamesWon = 1,
                                GamesLost = 0,
                                GamesWithScoreThreeNil = 0,
                                GamesWithScoreThreeOne = 1,
                                GamesWithScoreThreeTwo = 0,
                                GamesWithScoreTwoThree = 0,
                                GamesWithScoreOneThree = 0,
                                GamesWithScoreNilThree = 0,
                                SetsWon = 3,
                                SetsLost = 1,
                                SetsRatio = 3.0f / 1,
                                BallsWon = 102,
                                BallsLost = 96,
                                BallsRatio = 102.0f / 96
                            },
                            new StandingsEntryViewModel
                            {
                                TeamName = "TeamNameA",
                                Position = 3,
                                Points = 0,
                                GamesTotal = 2,
                                GamesWon = 0,
                                GamesLost = 2,
                                GamesWithScoreThreeNil = 0,
                                GamesWithScoreThreeOne = 0,
                                GamesWithScoreThreeTwo = 0,
                                GamesWithScoreTwoThree = 0,
                                GamesWithScoreOneThree = 2,
                                GamesWithScoreNilThree = 0,
                                SetsWon = 2,
                                SetsLost = 6,
                                SetsRatio = 2.0f / 6,
                                BallsWon = 192,
                                BallsLost = 204,
                                BallsRatio = 192.0f / 204
                            }
                        }
                    }
                },
                PivotTable = new List<PivotTableViewModel>()
                {
                    new PivotTableViewModel
                    {
                        TeamsStandings = GetPivotTeamsStandingsTwoTeamsScoresCompletelyEqual(),
                        AllGameResults = GetPivotTableTwoTeamsScoresCompletelyEqual()
                    }
                }
            };
            return this;
        }

        /// <summary>
        /// Sets the tournament's data with 2 teams scores completely equal.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithStandingsNotAvailableMessage()
        {
            _standingsViewModel = new StandingsViewModel
            {
                TournamentId = 4,
                TournamentName = "Name",
                Message = "Standings are not available for this tournament",
            };
            return this;
        }

        /// <summary>
        /// Sets the tournament's identifier of the view model.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithTournamentId(int id)
        {
            _standingsViewModel.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the tournament's name of the view model.
        /// </summary>
        /// <param name="name">Name of the tournament.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithTournamentName(string name)
        {
            _standingsViewModel.TournamentName = name;
            return this;
        }

        public StandingsViewModelBuilder WithLastUpdateTime(DateTime? lastUpdateTime)
        {
            // Update standings
            foreach (var standings in _standingsViewModel.StandingsTable)
            {
                standings.LastUpdateTime = lastUpdateTime;
            }

            // Update pivot
            foreach (var pivotTable in _standingsViewModel.PivotTable)
            {
                pivotTable.LastUpdateTime = lastUpdateTime;
            }

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="StandingsViewModelBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="StandingsViewModel"/>.</returns>
        public StandingsViewModel Build()
        {
            return _standingsViewModel;
        }

        private List<PivotTeamStandingsViewModel> GetPivotTeamsStandings()
        {
            var teams = new List<PivotTeamStandingsViewModel>
            {
                new PivotTeamStandingsViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 5,
                    SetsRatio = 6.0f / 3,
                    Position = 1,
                    BallsRatio = 234.0f / 214
                },
                new PivotTeamStandingsViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 3,
                    SetsRatio = 4.0f / 3,
                    Position = 2,
                    BallsRatio = 166.0f / 105
                },
                new PivotTeamStandingsViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 1,
                    SetsRatio = 2.0f / 6,
                    Position = 3,
                    BallsRatio = 123.0f / 204
                }
            };

            return teams;
        }

        private List<PivotTeamStandingsViewModel> GetPivotTeamsStandingsTwoTeamsScoresCompletelyEqual()
        {
            var teams = new List<PivotTeamStandingsViewModel>
            {
                new PivotTeamStandingsViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 3,
                    SetsRatio = 3.0f / 1,
                    Position = 1,
                    BallsRatio = 102.0f / 96
                },
                new PivotTeamStandingsViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 3,
                    SetsRatio = 3.0f / 1,
                    Position = 1,
                    BallsRatio = 102.0f / 96
                },
                new PivotTeamStandingsViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 0,
                    SetsRatio = 2.0f / 6,
                    Position = 3,
                    BallsRatio = 192.0f / 204
                }
            };

            return teams;
        }

        private List<PivotGameResultViewModel>[] GetPivotTableTwoTeamsScoresCompletelyEqual()
        {
            int rows = 9;
            var table = new List<PivotGameResultViewModel>[rows];
            for (int i = 0; i < rows; i++)
            {
                table[i] = new List<PivotGameResultViewModel>();
            }

            for (int i = 0; i < rows; i += 4)
            {
                table[i] = new List<PivotGameResultViewModel>();
                table[i].Add(new PivotGameResultViewModel
                {
                    HomeTeamId = 0,
                    AwayTeamId = 0,
                    HomeSetsScore = null,
                    AwaySetsScore = null,
                    IsTechnicalDefeat = false,
                    CssClass = CssClassConstants.NON_PLAYABLE_CELL
                });
            }

            table[2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[6].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            table[5].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[7].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            return table;
        }


        private List<PivotGameResultViewModel>[] GetPivotTable()
        {
            int rows = 9;
            var table = new List<PivotGameResultViewModel>[rows];
            for (int i = 0; i < rows; i++)
            {
                table[i] = new List<PivotGameResultViewModel>();
            }

            for (int i = 0; i < rows; i += 4)
            {
                table[i] = new List<PivotGameResultViewModel>();
                table[i].Add(new PivotGameResultViewModel
                {
                    HomeTeamId = 0,
                    AwayTeamId = 0,
                    HomeSetsScore = null,
                    AwaySetsScore = null,
                    IsTechnicalDefeat = false,
                    CssClass = CssClassConstants.NON_PLAYABLE_CELL
                });
            }

            table[1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_2
            });

            table[1].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeSetsScore = 0,
                AwaySetsScore = 3,
                IsTechnicalDefeat = true,
                CssClass = CssClassConstants.LOSS_0_3
            });

            table[3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeSetsScore = 2,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_2_3
            });

            table[3].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = true,
                CssClass = CssClassConstants.WIN_3_0
            });

            table[2].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeSetsScore = 3,
                AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.WIN_3_1
            });

            table[6].Add(new PivotGameResultViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeSetsScore = 1,
                AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.LOSS_1_3
            });

            return table;
        }
    }
}
