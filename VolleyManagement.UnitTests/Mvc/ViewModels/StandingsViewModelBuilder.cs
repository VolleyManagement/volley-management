namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;
    using VolleyManagement.UnitTests.Services.GameReportService;

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
                Standings = new List<StandingsEntryViewModel>
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
                },

                PivotTable = new PivotTableViewModel
                {
                    TeamsStandings = GetPivotTeamsStandings(),
                    AllGameResults = GetPivotTable()
                }
            };
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

        /// <summary>
        /// Sets the standings entries of the view model.
        /// </summary>
        /// <param name="entries">Entries of the standings.</param>
        /// <returns>Instance of <see cref="StandingsViewModelBuilder"/>.</returns>
        public StandingsViewModelBuilder WithEntries(List<StandingsEntryViewModel> entries)
        {
            _standingsViewModel.Standings = entries;
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
