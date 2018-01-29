namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a pivot table for one division
    /// </summary>
    public class PivotStandingsViewModel : DivisionStandingsViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsViewModel"/> class
        /// </summary>
        /// <param name="pivotStandings">Instance of a class which implements<see cref="PivotStandingsDto"/></param>
        public PivotStandingsViewModel(PivotStandingsDto pivotStandings)
        {
            LastUpdateTime = pivotStandings.LastUpdateTime;
            DivisionName = pivotStandings.DivisionName;
            TeamsStandings = pivotStandings.Teams.Select(PivotStandingsTeamViewModel.Map).ToList();

            // Group results by participating teams
            // As we do not care about home/away teams we use Min/Max ids to group results for T1vsT2 and T2vsT1 games together
            GamesStandings = pivotStandings.GameResults
                .GroupBy(gr =>
                    (TeamAId: Math.Min(gr.HomeTeamId, gr.AwayTeamId), TeamBId: Math.Max(gr.HomeTeamId, gr.AwayTeamId)))
                .Select(grouped => new PivotStandingsGameViewModel
                {
                    HomeTeamId = grouped.Key.TeamAId,
                    AwayTeamId = grouped.Key.TeamBId,
                    Results = grouped.Select(r =>
                    {
                        ShortGameResultViewModel result;
                        if (!r.WasPlayed)
                        {
                            result = ShortGameResultViewModel.CreatePlannedGame(r.RoundNumber);
                        }
                        else if (r.HomeTeamId == grouped.Key.TeamAId)
                        {
                            result = new ShortGameResultViewModel(
                             r.RoundNumber,
                             r.HomeGameScore,
                             r.AwayGameScore,
                             r.IsTechnicalDefeat);
                        }
                        else
                        {
                            // during grouping we got teams swapped
                            // need to revert sides
                            result = new ShortGameResultViewModel(
                             r.RoundNumber,
                             r.AwayGameScore,
                             r.HomeGameScore,
                             r.IsTechnicalDefeat);
                        }

                        return result;
                    }).ToList()
                }).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsViewModel"/> class
        /// </summary>
        public PivotStandingsViewModel()
        {
            TeamsStandings = new List<PivotStandingsTeamViewModel>();
            GamesStandings = new List<PivotStandingsGameViewModel>();
        }

        /// <summary>
        /// Gets or sets the ordered collection of teams.
        /// </summary>
        public List<PivotStandingsTeamViewModel> TeamsStandings { get; set; }

        /// <summary>
        /// Gets or sets the collection of games in tournament standings for pivot table.
        /// </summary>
        public List<PivotStandingsGameViewModel> GamesStandings { get; set; }
    }
}
