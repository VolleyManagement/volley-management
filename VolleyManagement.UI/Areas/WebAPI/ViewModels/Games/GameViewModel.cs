namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// GameViewModel class.
    /// </summary>
    public class GameViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class.
        /// </summary>
        public GameViewModel()
        {
            TotalScore = new Score();
            SetScores = Enumerable.Repeat(new Score(), Constants.GameResult.MAX_SETS_COUNT).ToList();
        }

        /// <summary>
        /// Gets or sets the identifier of game result.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the home team which played the game.
        /// </summary>
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Gets or sets the name of the away team which played the game.
        /// </summary>
        public string AwayTeamName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the game.
        /// </summary>
        public string GameDate { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game.
        /// </summary>
        public Score TotalScore { get; set; }
        
        /// <summary>
        /// Gets or sets the set scores.
        /// </summary>
        public List<Score> SetScores { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }
              
        /// <summary>
        /// Maps domain model of game result to view model of game.
        /// </summary>
        /// <param name="gameResult">Domain model of game.</param>
        /// <returns>View model of game.</returns>
        public static GameViewModel Map(GameResultDto gameResult)
        {
            int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            string offset = string.Format("{0}{1}", (hours > 0) ? "+" : string.Empty, hours.ToString("00"));

            return new GameViewModel
            {
                Id = gameResult.Id,
                HomeTeamName = gameResult.HomeTeamName,
                AwayTeamName = gameResult.AwayTeamName,
                GameDate = gameResult.GameDate.ToString("s") + offset,
                TotalScore = new Score { Home = gameResult.HomeSetsScore, Away = gameResult.AwaySetsScore },
                IsTechnicalDefeat = gameResult.IsTechnicalDefeat,
                SetScores = new List<Score>
                    {
                        new Score { Home = gameResult.HomeSet1Score, Away = gameResult.AwaySet1Score },
                        new Score { Home = gameResult.HomeSet2Score, Away = gameResult.AwaySet2Score },
                        new Score { Home = gameResult.HomeSet3Score, Away = gameResult.AwaySet3Score },
                        new Score { Home = gameResult.HomeSet4Score, Away = gameResult.AwaySet4Score },
                        new Score { Home = gameResult.HomeSet5Score, Away = gameResult.AwaySet5Score }
                    }
            };
        }

        /// <summary>
        /// Method checks whether property TotalScore can be serialized.
        /// </summary>
        /// <returns>True if property can be serialized; otherwise, false.</returns>
        public bool ShouldSerializeTotalScore()
        {
            return !this.TotalScore.IsEmpty();
        }

        /// <summary>
        /// Method checks whether property SetScores can be serialized.
        /// </summary>
        /// <returns>True if property can be serialized; otherwise, false.</returns>
        public bool ShouldSerializeSetScores()
        {
            return !this.TotalScore.IsEmpty();
        }

        /// <summary>
        /// Method checks whether property IsTechnicalDefeat can be serialized.
        /// </summary>
        /// <returns>True if property can be serialized; otherwise, false.</returns>
        public bool ShouldSerializeIsTechnicalDefeat()
        {
            return !this.TotalScore.IsEmpty();
        }
    }
}