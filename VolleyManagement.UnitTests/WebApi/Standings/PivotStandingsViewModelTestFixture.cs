namespace VolleyManagement.UnitTests.WebApi.Standings
{
    using System;
    using System.Collections.Generic;
    using UI.Areas.WebApi.ViewModels.GameReports;

    public class PivotStandingsViewModelTestFixture
    {
        private List<PivotStandingsViewModel> _pivotStandings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsViewModelTestFixture"/> class.
        /// </summary>
        public PivotStandingsViewModelTestFixture()
        {
            _pivotStandings = new List<PivotStandingsViewModel>
            {
                new PivotStandingsViewModel()
                {
                        TeamsStandings = GetPivotTeamsStandings(),
                        GamesStandings = GetGameStandings(),
                },
            };
        }

        public PivotStandingsViewModelTestFixture WithMultipleDivisionsAllPossibleScores()
        {
            _pivotStandings = new List<PivotStandingsViewModel>
                {
                    new PivotStandingsViewModel(),
                    new PivotStandingsViewModel()
            };

            AddPivotTeamsForMultipleDivisionsCase();

            AddFirstPivotResultsForMultipleDivisionsCase();
            AddSecondPivotResultsForMultipleDivisionsCase();

            return this;
        }

        public PivotStandingsViewModelTestFixture WithNotAllGamesPlayed()
        {
            _pivotStandings[0].TeamsStandings = new List<PivotStandingsTeamViewModel>
            {
                new PivotStandingsTeamViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 3,
                    SetsRatio = (float)3 / 3,
                },
                new PivotStandingsTeamViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 3,
                    SetsRatio = (float)3 / 3,
                },                
                new PivotStandingsTeamViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 0,
                    SetsRatio = null,
                },
            };

            _pivotStandings[0].GamesStandings = new List<PivotStandingsGameViewModel>
            {
                new PivotStandingsGameViewModel
                {
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    Results = new List<ShortGameResultViewModel>
                    {
                        new ShortGameResultViewModel(1, 3, 0),
                        new ShortGameResultViewModel(2, 0, 3),
                    },
                },
                new PivotStandingsGameViewModel
                {
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    Results = new List<ShortGameResultViewModel>
                    {
                        ShortGameResultViewModel.CreatePlannedGame(3),
                        ShortGameResultViewModel.CreatePlannedGame(4),
                    },
                },
                new PivotStandingsGameViewModel
                {
                    HomeTeamId = 2,
                    AwayTeamId = 3,
                    Results = new List<ShortGameResultViewModel>
                    {
                        ShortGameResultViewModel.CreatePlannedGame(5),
                        ShortGameResultViewModel.CreatePlannedGame(6),
                    },
                },
            };

            return this;
        }

        private void AddFirstPivotResultsForMultipleDivisionsCase()
        {
            // Group 1
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 0),
                    new ShortGameResultViewModel(2, 3),
                },
            });
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 1),
                    new ShortGameResultViewModel(1, 3),
                },
            });
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 2,
                AwayTeamId = 3,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 2),
                    new ShortGameResultViewModel(0, 3),
                },
            });

            // Group 2
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 5,
                AwayTeamId = 6,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 0),
                    new ShortGameResultViewModel(2, 3),
                },
            });
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 5,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(1, 3),
                    new ShortGameResultViewModel(3, 1),
                },
            });
            _pivotStandings[0].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 4,
                AwayTeamId = 6,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(2, 3),
                    new ShortGameResultViewModel(3, 0),
                },
            });
        }

        private void AddSecondPivotResultsForMultipleDivisionsCase()
        {
            _pivotStandings[1].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 8,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 0),
                    new ShortGameResultViewModel(2, 3),
                },
            });
            _pivotStandings[1].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 7,
                AwayTeamId = 9,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 1),
                    new ShortGameResultViewModel(1, 3),
                },
            });
            _pivotStandings[1].GamesStandings.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 8,
                AwayTeamId = 9,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 2),
                    new ShortGameResultViewModel(0, 3),
                },
            });
        }

        private void AddPivotTeamsForMultipleDivisionsCase()
        {
            _pivotStandings[0].TeamsStandings = new List<PivotStandingsTeamViewModel>();
            _pivotStandings[0].TeamsStandings.Add( // A
                new PivotStandingsTeamViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[0].TeamsStandings.Add( // E
                new PivotStandingsTeamViewModel
                {
                    TeamId = 5,
                    TeamName = "TeamNameE",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[0].TeamsStandings.Add( // C
                new PivotStandingsTeamViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[0].TeamsStandings.Add( // D
                new PivotStandingsTeamViewModel
                {
                    TeamId = 4,
                    TeamName = "TeamNameD",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[0].TeamsStandings.Add( // B
                new PivotStandingsTeamViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                });
            _pivotStandings[0].TeamsStandings.Add( // F
                new PivotStandingsTeamViewModel
                {
                    TeamId = 6,
                    TeamName = "TeamNameF",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                });

            _pivotStandings[1].TeamsStandings = new List<PivotStandingsTeamViewModel>();
            _pivotStandings[1].TeamsStandings.Add( // G
                new PivotStandingsTeamViewModel
                {
                    TeamId = 7,
                    TeamName = "TeamNameG",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[1].TeamsStandings.Add( // I
                new PivotStandingsTeamViewModel
                {
                    TeamId = 9,
                    TeamName = "TeamNameI",
                    Points = 7,
                    SetsRatio = (float)9 / 7,
                });
            _pivotStandings[1].TeamsStandings.Add( // H
                new PivotStandingsTeamViewModel
                {
                    TeamId = 8,
                    TeamName = "TeamNameH",
                    Points = 4,
                    SetsRatio = (float)6 / 10,
                });
            _pivotStandings[1].TeamsStandings.Add( // J
                new PivotStandingsTeamViewModel
                {
                    TeamId = 10,
                    TeamName = "TeamNameJ",
                    Points = 0,
                    SetsRatio = null,
                });
            _pivotStandings[1].TeamsStandings.Add( // K
                new PivotStandingsTeamViewModel
                {
                    TeamId = 11,
                    TeamName = "TeamNameK",
                    Points = 0,
                    SetsRatio = null,
                });
            _pivotStandings[1].TeamsStandings.Add( // L
                new PivotStandingsTeamViewModel
                {
                    TeamId = 12,
                    TeamName = "TeamNameL",
                    Points = 0,
                    SetsRatio = null,
                });

        }

        public PivotStandingsViewModelTestFixture WithLastUpdateTime(DateTime? lastUpdateTime)
        {
            // Update pivot
            foreach (var pivotTable in _pivotStandings)
            {
                pivotTable.LastUpdateTime = lastUpdateTime;
            }

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="PivotStandingsViewModelTestFixture"/>.
        /// </summary>
        public List<PivotStandingsViewModel> Build()
        {
            return _pivotStandings;
        }

        private List<PivotStandingsTeamViewModel> GetPivotTeamsStandings()
        {
            var teams = new List<PivotStandingsTeamViewModel>
            {
                new PivotStandingsTeamViewModel
                {
                    TeamId = 1,
                    TeamName = "TeamNameA",
                    Points = 5,
                    SetsRatio = 6.0f / 3
                },
                new PivotStandingsTeamViewModel
                {
                    TeamId = 3,
                    TeamName = "TeamNameC",
                    Points = 3,
                    SetsRatio = 4.0f / 3
                },
                new PivotStandingsTeamViewModel
                {
                    TeamId = 2,
                    TeamName = "TeamNameB",
                    Points = 1,
                    SetsRatio = 2.0f / 6
                }
            };

            return teams;
        }

        private List<PivotStandingsGameViewModel> GetGameStandings()
        {
            var table = new List<PivotStandingsGameViewModel>();

            table.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 3,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 2),
                    new ShortGameResultViewModel(0, 3)
                }
            });

            table.Add(new PivotStandingsGameViewModel
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                Results = new List<ShortGameResultViewModel>
                {
                    new ShortGameResultViewModel(3, 1)
                }
            });

            return table;
        }
    }
}