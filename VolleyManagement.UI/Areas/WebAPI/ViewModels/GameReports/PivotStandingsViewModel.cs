namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="PivotStandingsViewModel"/>.
    /// </summary>
    public class PivotStandingsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsViewModel"/> class
        /// </summary>
        /// <param name="pivotStandings">Instance of a class which implements<see cref="PivotStandingsDto"/></param>
        public PivotStandingsViewModel(PivotStandingsDto pivotStandings)
        {
            TeamsStandings = pivotStandings.Teams.Select(t => PivotStandingsEntryViewModel.Map(t)).ToList();
            GamesStandings = pivotStandings.GameResults.Select(g => PivotStandingsGameViewModel.Map(g)).ToList();

            foreach (var game in GamesStandings)
            {
                game.Results = pivotStandings.GameResults
                    .Where(g => g.HomeTeamId == game.HomeTeamId && g.AwayTeamId == game.AwayTeamId)
                    .Select(g => ShortGameResultViewModel.Map(g)).ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsViewModel"/> class
        /// </summary>
        public PivotStandingsViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the ordered collection of teams.
        /// </summary>
        public List<PivotStandingsEntryViewModel> TeamsStandings { get; set; }

        /// <summary>
        /// Gets or sets the collection of games in tournament standings for pivot table.
        /// </summary>
        public List<PivotStandingsGameViewModel> GamesStandings { get; set; }
    }
}