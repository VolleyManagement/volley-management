namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="PivotTableViewModel"/>.
    /// </summary>
    public class PivotTableViewModel
    {
        private const byte ZERO = 0;
        private const byte ONE = 1;
        private const byte TWO = 2;
        private const byte THREE = 3;

        /// <summary>
        /// Gets or sets the ordered collection of teams according their position in tournament.
        /// </summary>
        public List<PivotTeamStandingsViewModel> TeamsStandings { get; set; }

        /// <summary>
        /// Gets or sets the collection of games in tournament standings for pivot table.
        /// </summary>
        public List<PivotGameResultsViewModel>[,] GameResults { get; set; }

        /// <summary>
        /// Create game results pivot table
        /// </summary>
        /// <param name="gameResults">Collection of all results in tournament</param>
        /// <param name="teams">Collection of teams in tournament</param>
        /// <returns>Pivot table with all results</returns>
        public static List<PivotGameResultsViewModel>[,] CreateGameResultsTable(
            IReadOnlyCollection<ShortGameResultDto> gameResults,
            IReadOnlyCollection<TeamStandingsDto> teams)
        {
            int columns = teams.Count;
            int rows = teams.Count;

            List<PivotGameResultsViewModel>[,] resultsTable = new List<PivotGameResultsViewModel>[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = i; j < columns; j++)
                {
                    if (i == j)
                    {
                        resultsTable[i, j] = GetNonPlayableCell();
                        resultsTable[j, i] = GetNonPlayableCell();
                    }
                    else
                    {
                        int rowTeamId = teams.ElementAt(i).TeamId;
                        int columnTeamId = teams.ElementAt(j).TeamId;

                        var results = GetHomeGames(gameResults, rowTeamId, columnTeamId);
                        resultsTable[i, j] = results;
                        results = GetCellValueForOpponentTeam(results);
                        resultsTable[j, i] = results;

                        results = GetAwayGames(gameResults, rowTeamId, columnTeamId);
                        resultsTable[j, i].AddRange(results);
                        results = GetCellValueForOpponentTeam(results);
                        resultsTable[i, j].AddRange(results);
                    }
                }
            }

            return resultsTable;
        }

        /// <summary>
        /// According game score returns cascade style sheets class name
        /// </summary>
        /// <param name="homeScore">Sets score of home team</param>
        /// <param name="awayScore">Sets score of away team</param>
        /// <returns>Name of the class with cascade style sheets settings</returns>
        public static string SetCssClass(byte? homeScore, byte? awayScore)
        {
            if (homeScore == THREE && awayScore == ZERO)
            {
                return CssClassConstants.WIN_3_0;
            }
            else if (homeScore == THREE && awayScore == ONE)
            {
                return CssClassConstants.WIN_3_1;
            }
            else if (homeScore == THREE && awayScore == TWO)
            {
                return CssClassConstants.WIN_3_2;
            }
            else if (homeScore == TWO && awayScore == THREE)
            {
                return CssClassConstants.LOSE_2_3;
            }
            else if (homeScore == ONE && awayScore == THREE)
            {
                return CssClassConstants.LOSE_1_3;
            }
            else if (homeScore == ZERO && awayScore == THREE)
            {
                return CssClassConstants.LOSE_0_3;
            }

            return CssClassConstants.NORESULT;
        }

        private static List<PivotGameResultsViewModel> GetAwayGames(
            IReadOnlyCollection<ShortGameResultDto> allResults,
            int homeTeamId,
            int awayTeamId)
        {
            return GetHomeGames(allResults, awayTeamId, homeTeamId);
        }

        private static List<PivotGameResultsViewModel> GetHomeGames(
            IReadOnlyCollection<ShortGameResultDto> allResults,
            int homeTeamId,
            int awayTeamId)
        {
            return allResults.Where(r => r.HomeTeamId == homeTeamId && r.AwayTeamId == awayTeamId)
                .Select(PivotGameResultsViewModel.GetPivotGameResultsViewModelMapper).ToList();
        }

        private static List<PivotGameResultsViewModel> GetNonPlayableCell()
        {
            return new List<PivotGameResultsViewModel>
            {
                new PivotGameResultsViewModel
                {
                     HomeTeamId = 0,
                     AwayTeamId = 0,
                     HomeSetsScore = null,
                     AwaySetsScore = null,
                     IsTechnicalDefeat = false,
                     CssClass = CssClassConstants.NON_PLAYABLE_CELL
                }
            };
        }

        private static List<PivotGameResultsViewModel> GetCellValueForOpponentTeam(List<PivotGameResultsViewModel> results)
        {
            List<PivotGameResultsViewModel> swapedResults = new List<PivotGameResultsViewModel>();
            foreach (var item in results)
            {
                PivotGameResultsViewModel tmp = new PivotGameResultsViewModel();
                tmp.HomeTeamId = item.AwayTeamId;
                tmp.AwayTeamId = item.HomeTeamId;
                tmp.HomeSetsScore = item.AwaySetsScore;
                tmp.AwaySetsScore = item.HomeSetsScore;
                tmp.IsTechnicalDefeat = item.IsTechnicalDefeat;
                tmp.CssClass = SetCssClass(tmp.HomeSetsScore, tmp.AwaySetsScore);
                swapedResults.Add(tmp);
            }

            return swapedResults;
        }
    }
}