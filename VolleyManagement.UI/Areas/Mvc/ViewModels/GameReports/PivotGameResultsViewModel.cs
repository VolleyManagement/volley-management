namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="PivotGameResultViewModel"/>.
    /// </summary>
    public class PivotGameResultViewModel
    {
        private const byte ZERO = 0;
        private const byte ONE = 1;
        private const byte TWO = 2;
        private const byte THREE = 3;

        /// <summary>
        /// Gets or sets the identifier of the home team which played the game.
        /// </summary>
        public int HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the home team.
        /// </summary>
        public byte? HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte? AwaySetsScore { get; set; }

        /// <summary>
        /// Gets the total score of the game.
        /// </summary>
        public string FormattedResult
        {
            get
            {
                string result = HomeSetsScore + " : " + AwaySetsScore;
                if (IsTechnicalDefeat)
                {
                    result += "*";
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a cascade style sheets class name with style settings.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Maps domain model of game results to view model of game results for pivot table.
        /// </summary>
        /// <param name="game">Domain model of short game result.</param>
        /// <returns>View model of game result for pivot table.</returns>
        public static PivotGameResultViewModel Map(ShortGameResultDto game)
        {
            return new PivotGameResultViewModel
            {
                HomeTeamId = game.HomeTeamId,
                AwayTeamId = game.AwayTeamId,
                HomeSetsScore = game.HomeSetsScore,
                AwaySetsScore = game.AwaySetsScore,
                IsTechnicalDefeat = game.IsTechnicalDefeat,
                CssClass = GetCssClass(game.HomeSetsScore, game.AwaySetsScore)
            };
        }

        /// <summary>
        /// Get cell with data for situation when no games can be played(team can not play against itself)
        /// </summary>
        /// <returns>List with special data for non playable game</returns>
        public static PivotGameResultViewModel GetNonPlayableCell()
        {
            return new PivotGameResultViewModel
            {
                HomeTeamId = 0,
                AwayTeamId = 0,
                HomeSetsScore = null,
                AwaySetsScore = null,
                IsTechnicalDefeat = false,
                CssClass = CssClassConstants.NON_PLAYABLE_CELL
            };
        }

        /// <summary>
        /// Reverse teams in game result
        /// </summary>
        /// <returns>Game results for opponent team</returns>
        public PivotGameResultViewModel ReverseTeams()
        {
            var result = new PivotGameResultViewModel();
            result.HomeTeamId = this.AwayTeamId;
            result.AwayTeamId = this.HomeTeamId;
            result.HomeSetsScore = this.AwaySetsScore;
            result.AwaySetsScore = this.HomeSetsScore;
            result.IsTechnicalDefeat = this.IsTechnicalDefeat;
            result.CssClass = GetCssClass(result.HomeSetsScore, result.AwaySetsScore);
            return result;
        }

        /// <summary>
        /// According game score returns cascade style sheets class name
        /// </summary>
        /// <param name="homeScore">Sets score of home team</param>
        /// <param name="awayScore">Sets score of away team</param>
        /// <returns>Name of the class with cascade style sheets settings</returns>
        private static string GetCssClass(byte? homeScore, byte? awayScore)
        {
            string cssClass = CssClassConstants.NORESULT;
            if (homeScore == THREE && awayScore == ZERO)
            {
                cssClass = CssClassConstants.WIN_3_0;
            }
            else if (homeScore == THREE && awayScore == ONE)
            {
                cssClass = CssClassConstants.WIN_3_1;
            }
            else if (homeScore == THREE && awayScore == TWO)
            {
                cssClass = CssClassConstants.WIN_3_2;
            }
            else if (homeScore == TWO && awayScore == THREE)
            {
                cssClass = CssClassConstants.LOSS_2_3;
            }
            else if (homeScore == ONE && awayScore == THREE)
            {
                cssClass = CssClassConstants.LOSS_1_3;
            }
            else if (homeScore == ZERO && awayScore == THREE)
            {
                cssClass = CssClassConstants.LOSS_0_3;
            }

            return cssClass;
        }
    }
}
