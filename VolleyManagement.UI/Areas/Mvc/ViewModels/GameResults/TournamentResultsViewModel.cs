namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for tournament results.
    /// </summary>
    public class TournamentResultsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentResultsViewModel"/> class.
        /// </summary>
        public TournamentResultsViewModel()
        {
            GameResults = new List<GameResultViewModel>();
        }

        /// <summary>
        /// Gets or sets the identifier of the tournament.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tournament.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the game results related to current tournament.
        /// </summary>
        public List<GameResultViewModel> GameResults { get; set; }
    }
}
