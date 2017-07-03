namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Authorization;
    using Domain.TournamentsAggregate;
    using GameResults;

    /// <summary>
    /// Represents tournaments schedule
    /// </summary>
    public class ScheduleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleViewModel"/> class
        /// </summary>
        public ScheduleViewModel()
        {
            Rounds = new Dictionary<byte, List<GameResultViewModel>>();
        }

        /// <summary>
        /// Gets or sets number of rounds in tournament
        /// </summary>
        public byte NumberOfRounds { get; set; }

        /// <summary>
        /// Gets or sets current rounds collection
        /// </summary>
        public Dictionary<byte, List<GameResultViewModel>> Rounds { get; set; }

        /// <summary>
        /// Gets or sets id of tournament
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets name of tournament
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Gets or sets tournament's scheme
        /// </summary>
        public TournamentSchemeEnum TournamentScheme { get; set; }

        /// <summary>
        /// Gets or sets names of rounds for playoff scheme
        /// </summary>
        public string[] RoundNames { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> create object
        /// </summary>
        public AllowedOperations AllowedOperations { get; set; }

        /// <summary>
        /// Checks if the game is the final game (for playoff scheme)
        /// </summary>
        /// <param name="game">Game to check</param>
        /// <returns>True, if game is the final</returns>
        public bool IsFinal(GameResultViewModel game)
        {
            return NumberOfRounds != 0 &&
                   game.GameNumber == Rounds.Last().Value.Last().GameNumber;
        }

        /// <summary>
        /// Checks if the game is the bronze match (for playoff scheme)
        /// </summary>
        /// <param name="game">Game to check</param>
        /// <returns>True, if game is the bronze match</returns>
        public bool IsBronzeMatch(GameResultViewModel game)
        {
            return NumberOfRounds != 0 &&
                   game.GameNumber == Rounds.Last().Value.Last().GameNumber - 1;
        }
    }
}