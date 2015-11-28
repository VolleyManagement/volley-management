namespace VolleyManagement.Domain.GameResultsAggregate
{
    using System;

    /// <summary>
    /// Game result validation class.
    /// </summary>
    public static class GameResultValidation
    {
        /// <summary>
        /// Determines whether the sets score and the scores of every set match one another.
        /// </summary>
        /// <param name="homeSetsScore">Sets score of the home team.</param>
        /// <param name="awaySetsScore">Sets score of the away team.</param>
        /// <param name="homeSetScores">Scores of every set home team played.</param>
        /// <param name="awaySetScores">Scores of every set home away played.</param>
        /// <returns>True if sets score and scores of every set match; otherwise, false.</returns>
        public static bool AreSetScoresMatched(byte homeSetsScore, byte awaySetsScore, byte[] homeSetScores, byte[] awaySetScores)
        {
            byte homeScore = 0;
            byte awayScore = 0;

            for (int i = 0; i < homeSetScores.Length; i++)
            {
                if (homeSetScores[i] > awaySetScores[i])
                {
                    homeScore++;
                }
                else
                {
                    awayScore++;
                }
            }

            return homeSetsScore == homeScore && awaySetsScore == awayScore;
        }

        /// <summary>
        /// Determines whether the sets score is valid.
        /// </summary>
        /// <param name="homeSetsScore">Sets score of the home team.</param>
        /// <param name="awaySetsScore">Sets score of the away team.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <returns>True if sets score is valid; otherwise, false.</returns>
        public static bool IsSetsScoreValid(byte homeSetsScore, byte awaySetsScore, bool isTechnicalDefeat)
        {
            return isTechnicalDefeat
                ? IsTechnicalDefeatSetsScoreValid(homeSetsScore, awaySetsScore)
                : IsOrdinarySetsScoreValid(homeSetsScore, awaySetsScore);
        }

        /// <summary>
        /// Determines whether the score of a required set is valid.
        /// </summary>
        /// <param name="homeSetScore">Set score of the home team.</param>
        /// <param name="awaySetScore">Set score of the away team.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <returns>True if required set score is valid; otherwise, false.</returns>
        public static bool IsRequiredSetScoreValid(byte homeSetScore, byte awaySetScore, bool isTechnicalDefeat)
        {
            return isTechnicalDefeat
                ? IsTechnicalDefeatRequiredSetScoreValid(homeSetScore, awaySetScore)
                : IsOrdinarySetScoreValid(homeSetScore, awaySetScore);
        }

        /// <summary>
        /// Determines whether the score of an optional set is valid.
        /// </summary>
        /// <param name="homeSetScore">Set score of the home team.</param>
        /// <param name="awaySetScore">Set score of the away team.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <returns>True if optional set score is valid; otherwise, false.</returns>
        public static bool IsOptionalSetScoreValid(byte homeSetScore, byte awaySetScore, bool isTechnicalDefeat)
        {
            return isTechnicalDefeat
                ? IsTechnicalDefeatOptionalSetScoreValid(homeSetScore, awaySetScore)
                : IsOrdinarySetScoreValid(homeSetScore, awaySetScore);
        }

        private static bool IsTechnicalDefeatSetsScoreValid(byte homeSetsScore, byte awaySetsScore)
        {
            return (homeSetsScore == Constants.GameResult.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
                && awaySetsScore == Constants.GameResult.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
                || (homeSetsScore == Constants.GameResult.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
                && awaySetsScore == Constants.GameResult.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
        }

        private static bool IsOrdinarySetsScoreValid(byte homeSetsScore, byte awaySetsScore)
        {
            return homeSetsScore + awaySetsScore >= Constants.GameResult.MIN_SETS_COUNT
                && homeSetsScore + awaySetsScore <= Constants.GameResult.MAX_SETS_COUNT;
        }

        private static bool IsTechnicalDefeatRequiredSetScoreValid(byte homeSetScore, byte awaySetScore)
        {
            return (homeSetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_WINNER_SCORE
                && awaySetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
                || (homeSetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE
                && awaySetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
        }

        private static bool IsTechnicalDefeatOptionalSetScoreValid(byte homeSetScore, byte awaySetScore)
        {
            return homeSetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE
                && awaySetScore == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE;
        }

        private static bool IsOrdinarySetScoreValid(byte homeSetScore, byte awaySetScore)
        {
            return (homeSetScore >= Constants.GameResult.MARGIN_SET_POINTS
                    || awaySetScore >= Constants.GameResult.MARGIN_SET_POINTS)
                    && Math.Abs(awaySetScore - homeSetScore) == Constants.GameResult.POINTS_DELTA_TO_WIN;
        }
    }
}
