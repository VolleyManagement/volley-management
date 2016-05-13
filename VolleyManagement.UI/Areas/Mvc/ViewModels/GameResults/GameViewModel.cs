namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Game to be scheduled in the tournament view model
    /// </summary>
    public class GameViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class
        /// </summary>
        public GameViewModel()
        {
        }

        /// <summary>
        /// Gets or sets id of a game
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets list of teams to be selected
        /// </summary>
        public IEnumerable<SelectListItem> Teams { get; set; }

        /// <summary>
        /// Gets or sets list of rounds to be selected
        /// </summary>
        public SelectList Rounds { get; set; }

        /// <summary>
        /// Gets or sets round number of the game
        /// </summary>
        [Display(Name = "Round", ResourceType = typeof(ViewModelResources))]
        public byte Round { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tournament where game belongs.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the home team.
        /// </summary>
        [Display(Name = "HomeTeam", ResourceType = typeof(ViewModelResources))]
        public int HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team.
        /// </summary>
        [Display(Name = "AwayTeam", ResourceType = typeof(ViewModelResources))]
        public int? AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets date of the game.
        /// </summary>
        [DataType(DataType.DateTime)]
        [Display(Name = "GameDateTime", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public DateTime GameDate { get; set; }

        /// <summary>
        /// Mapper from GameResult to GameViewModel
        /// </summary>
        /// <param name="game">GameResult to map from</param>
        /// <returns>Complete GameViewModel</returns>
        public static GameViewModel Map(GameResultDto game)
        {
            return new GameViewModel
            {
                Id = game.Id,
                TournamentId = game.TournamentId,
                HomeTeamId = game.HomeTeamId,
                AwayTeamId = game.AwayTeamId,
                Round = game.Round,
                GameDate = game.GameDate
            };
        }

        /// <summary>
        /// Mapper from <see cref="GameViewModel"/> to <see cref="Game"/>
        /// </summary>
        /// <returns>Game domain model</returns>
        public Game ToDomain()
        {
            return new Game
            {
                Id = this.Id,
                Round = this.Round,
                TournamentId = this.TournamentId,
                HomeTeamId = this.HomeTeamId,
                AwayTeamId = this.AwayTeamId,
                GameDate = this.GameDate
            };
        }
    }
}