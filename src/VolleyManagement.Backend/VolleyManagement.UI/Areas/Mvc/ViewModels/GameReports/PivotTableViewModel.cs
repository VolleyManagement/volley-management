namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="PivotTableViewModel"/>.
    /// </summary>
    public class PivotTableViewModel : DivisionStandingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PivotTableViewModel"/> class
        /// </summary>
        /// <param name="pivotStandings">Instance of a class which implements<see cref="PivotStandingsDto"/></param>
        public PivotTableViewModel(PivotStandingsDto pivotStandings)
        {
            LastUpdateTime = pivotStandings.LastUpdateTime;

            TeamsStandings = pivotStandings.Teams.Select(PivotTeamStandingsViewModel.Map).ToList();
            TeamStandingsViewModelBase.SetPositions(TeamsStandings);

            GameResults = pivotStandings.GameResults.Select(PivotGameResultViewModel.Map).ToList();
            AllGameResults = new List<PivotGameResultViewModel>[TeamsStandings.Count * TeamsStandings.Count];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PivotTableViewModel"/> class.
        /// </summary>
        public PivotTableViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the ordered collection of teams according their position in tournament.
        /// </summary>
        public List<PivotTeamStandingsViewModel> TeamsStandings { get; set; }

        /// <summary>
        /// Gets or sets the collection of games in tournament standings for pivot table.
        /// </summary>
        public List<PivotGameResultViewModel>[] AllGameResults { get; set; }

        private List<PivotGameResultViewModel> GameResults { get; set; }

        /// <summary>
        /// Indexer to get game results
        /// </summary>
        /// <param name="i">row index</param>
        /// <param name="j">column index</param>
        /// <returns>Collection of game results</returns>
        public List<PivotGameResultViewModel> this[int i, int j]
        {
            get
            {
                var results = new List<PivotGameResultViewModel>();
                if (i == j)
                {
                    results.Add(PivotGameResultViewModel.GetNonPlayableCell());
                    SetCellValue(i, j, results);
                }
                else
                {
                    if (AllGameResults[(j * TeamsStandings.Count) + i] != null)
                    {
                        var result = AllGameResults[(j * TeamsStandings.Count) + i];
                        results.AddRange(result.ConvertAll(gr => gr.ReverseTeams()));
                        SetCellValue(i, j, results);
                    }
                    else
                    {
                        int rowTeamId = TeamsStandings[i].TeamId;
                        int columnTeamId = TeamsStandings[j].TeamId;
                        results = GetAllResultsForTeams(rowTeamId, columnTeamId);
                        SetCellValue(i, j, results);
                    }
                }

                return results;
            }
        }

        private List<PivotGameResultViewModel> GetAllResultsForTeams(int rowTeamId, int columnTeamId)
        {
            var results = new List<PivotGameResultViewModel>();
            var resultHome = GameResults.Where(r => r.HomeTeamId == rowTeamId && r.AwayTeamId == columnTeamId).ToList();
            var resultAway = GameResults.Where(r => r.AwayTeamId == rowTeamId && r.HomeTeamId == columnTeamId).ToList();
            results.AddRange(resultHome);
            results.AddRange(resultAway.ConvertAll(gr => gr.ReverseTeams()));
            return results;
        }

        private void SetCellValue(int i, int j, IEnumerable<PivotGameResultViewModel> result)
        {
            AllGameResults[(i * TeamsStandings.Count) + j] = new List<PivotGameResultViewModel>(result);
        }
    }
}
