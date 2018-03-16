namespace VolleyManagement.Domain.GamesAggregate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result validation class.
    /// </summary>
    public static class ResultValidation
    {
        /// <summary>
        /// Determines whether the sets score is valid.
        /// </summary>
        /// <param name="setsScore">Sets score (final score) of the game.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <returns>True if sets score is valid; otherwise, false.</returns>
        public static bool IsSetsScoreValid(Score setsScore, bool isTechnicalDefeat)
        {
            return isTechnicalDefeat ? IsTechnicalDefeatSetsScoreValid(setsScore) : IsOrdinarySetsScoreValid(setsScore);
        }

        /// <summary>
        /// Determines whether the sets score and the scores of every set match one another.
        /// </summary>
        /// <param name="setsScore">Sets score (final score) of the game.</param>
        /// <param name="setScores">Scores of every set of the game.</param>
        /// <returns>True if sets score and scores of every set match; otherwise, false.</returns>
        public static bool AreSetScoresMatched(Score setsScore, IList<Score> setScores)
        {
            if (setScores == null)
            {
                return false;
            }

            Score score = new Score();

            foreach (var setScore in setScores)
            {
                if (setScore.Home > setScore.Away)
                {
                    score.Home++;
                }
                else if (setScore.Home < setScore.Away)
                {
                    score.Away++;
                }
            }

            return score.Home == setsScore.Home && score.Away == setsScore.Away;
        }

        /// <summary>
        /// Determines whether the score of a required set is valid.
        /// </summary>
        /// <param name="setScore">Score of the set.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <returns>True if required set score is valid; otherwise, false.</returns>
        public static bool IsRequiredSetScoreValid(Score setScore, bool isTechnicalDefeat)
        {
            return isTechnicalDefeat ? IsTechnicalDefeatRequiredSetScoreValid(setScore) : IsOrdinaryRequiredSetScoreValid(setScore);
        }

        /// <summary>
        /// Determines whether the score of an optional set is valid.
        /// </summary>
        /// <param name="setScore">Score of the set.</param>
        /// <param name="isTechnicalDefeat">Value indicating whether the technical defeat has taken place.</param>
        /// <param name="setOrderNumber">Value indicating set order number.</param>
        /// <returns>True if optional set score is valid; otherwise, false.</returns>
        public static bool IsOptionalSetScoreValid(Score setScore, bool isTechnicalDefeat, int setOrderNumber)
        {
            return isTechnicalDefeat ? IsTechnicalDefeatOptionalSetScoreValid(setScore) :
                IsOrdinaryOptionalSetScoreValid(setScore, setOrderNumber);
        }

        /// <summary>
        /// Determines whether the set is not played.
        /// </summary>
        /// <param name="setScore">Score of the set.</param>
        /// <returns>True if set is not played; otherwise, false.</returns>
        public static bool IsSetUnplayed(Score setScore)
        {
            return setScore.Home == Constants.GameResult.UNPLAYED_SET_HOME_SCORE
                && setScore.Away == Constants.GameResult.UNPLAYED_SET_AWAY_SCORE;
        }


        private static bool IsSetScoreValid(Score score, Score setScore, ref bool hasMatchEnded)
        {
            if (setScore.Home > setScore.Away)
            {
                if (hasMatchEnded)
                {
                    return false;
                }

                score.Home++;

                if (score.Home == Constants.GameResult.SETS_COUNT_TO_WIN)
                {
                    hasMatchEnded = true;
                }
            }
            else if (setScore.Home < setScore.Away)
            {
                if (hasMatchEnded)
                {
                    return false;
                }

                score.Away++;

                if (score.Away == Constants.GameResult.SETS_COUNT_TO_WIN)
                {
                    hasMatchEnded = true;
                }
            }

            return true;
        }
        /// <summary>
        /// Determines whether the set scores are listed in the correct order.
        /// </summary>
        /// <param name="setScores">Scores of every set of the game.</param>
        /// <returns>True if set scores are listed in the correct order; otherwise, false.</returns>
        public static bool AreSetScoresOrdered(IList<Score> setScores)
        {
            Score score = new Score();
            bool hasMatchEnded = false;

            foreach (var setScore in setScores)
            {
                if (!IsSetScoreValid(score, setScore, ref hasMatchEnded))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsTechnicalDefeatSetsScoreValid(Score setsScore)
        {
            return (setsScore.Home == Constants.GameResult.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
                && setsScore.Away == Constants.GameResult.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
                || (setsScore.Home == Constants.GameResult.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
                && setsScore.Away == Constants.GameResult.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
        }

        private static bool IsOrdinarySetsScoreValid(Score setsScore)
        {
            return (setsScore.Home == Constants.GameResult.SETS_COUNT_TO_WIN
                && setsScore.Away < Constants.GameResult.SETS_COUNT_TO_WIN)
                || (setsScore.Home < Constants.GameResult.SETS_COUNT_TO_WIN
                && setsScore.Away == Constants.GameResult.SETS_COUNT_TO_WIN);
        }

        private static bool IsTechnicalDefeatRequiredSetScoreValid(Score setScore)
        {
            return (setScore.Home == Constants.GameResult.TECHNICAL_DEFEAT_SET_WINNER_SCORE
                && setScore.Away == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
                || (setScore.Home == Constants.GameResult.TECHNICAL_DEFEAT_SET_LOSER_SCORE
                && setScore.Away == Constants.GameResult.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
        }

        private static bool IsTechnicalDefeatOptionalSetScoreValid(Score setScore)
        {
            return setScore.Home == Constants.GameResult.UNPLAYED_SET_HOME_SCORE
                && setScore.Away == Constants.GameResult.UNPLAYED_SET_AWAY_SCORE;
        }

        private static bool IsOrdinaryRequiredSetScoreValid(Score setScore, int setOrderNumber = 0)
        {
            byte defScoreToWin = 0;

            defScoreToWin = setOrderNumber == Constants.GameResult.MAX_SETS_COUNT ?
                (byte)Constants.GameResult.FIFTH_SET_POINTS_MIN_VALUE_TO_WIN :
                (byte)Constants.GameResult.SET_POINTS_MIN_VALUE_TO_WIN;

            return IsSetScoreValid(setScore, defScoreToWin);
        }

        private static bool IsSetScoreValid(Score setScore, int minSetScore)
        {
            bool isValid = false;

            if (IsSetScoreGreaterThanMin(setScore, minSetScore))
            {
                isValid = IsPointsDifferenceEqualRequired(setScore);
            }
            else if (IsSetScoreEqualToMin(setScore, minSetScore))
            {
                isValid = IsPointsDifferenceGreaterOrEqualRequired(setScore);
            }

            return isValid;
        }

        private static bool IsPointsDifferenceEqualRequired(Score setScore)
        {
            return Math.Abs(setScore.Home - setScore.Away) == Constants.GameResult.SET_POINTS_MIN_DELTA_TO_WIN;
        }

        private static bool IsPointsDifferenceGreaterOrEqualRequired(Score setScore)
        {
            return Math.Abs(setScore.Home - setScore.Away) >= Constants.GameResult.SET_POINTS_MIN_DELTA_TO_WIN;
        }

        private static bool IsOrdinaryOptionalSetScoreValid(Score setScore, int setOrderNumber)
        {
            return IsOrdinaryRequiredSetScoreValid(setScore, setOrderNumber) || IsSetUnplayed(setScore);
        }

        private static bool IsSetScoreEqualToMin(Score setScore, int minSetScore)
        {
            return setScore.Home == minSetScore
                || setScore.Away == minSetScore;
        }

        private static bool IsSetScoreGreaterThanMin(Score setScore, int minSetScore)
        {
            return setScore.Home > minSetScore
                || setScore.Away > minSetScore;
        }
    }
}