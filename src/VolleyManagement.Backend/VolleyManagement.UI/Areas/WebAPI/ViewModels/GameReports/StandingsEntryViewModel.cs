namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="StandingsEntry"/>.
    /// </summary>
    public class StandingsEntryViewModel
    {
        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets the team's position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the number of point for the team.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the total number of games played by the team.
        /// </summary>
        public int GamesTotal { get; set; }

        /// <summary>
        /// Gets or sets the total number of games the team won.
        /// </summary>
        public int? GamesWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of games the team lost.
        /// </summary>
        public int? GamesLost { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to nil (3:0).
        /// </summary>
        public int? GamesWithScoreThreeNil { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to one (3:1).
        /// </summary>
        public int? GamesWithScoreThreeOne { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to two (3:2).
        /// </summary>
        public int? GamesWithScoreThreeTwo { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score two to three (2:3).
        /// </summary>
        public int? GamesWithScoreTwoThree { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score one to three (1:3).
        /// </summary>
        public int? GamesWithScoreOneThree { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score nil to three (0:3).
        /// </summary>
        public int? GamesWithScoreNilThree { get; set; }

        /// <summary>
        /// Gets or sets the total number of sets the team won.
        /// </summary>
        public int? SetsWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of sets the team lost.
        /// </summary>
        public int? SetsLost { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float? SetsRatio { get; set; }

        /// <summary>
        /// Gets or sets the total number of balls the team won.
        /// </summary>
        public int? BallsWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of balls the team lost.
        /// </summary>
        public int? BallsLost { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of balls the team won to number of balls the team lost.
        /// </summary>
        public float? BallsRatio { get; set; }

        /// <summary>
        /// Maps domain model of <see cref="StandingsEntry"/> to view model of <see cref="StandingsEntry"/>.
        /// </summary>
        /// <param name="standingsEntry">Domain model of <see cref="StandingsEntry"/>.</param>
        /// <returns>View model of <see cref="StandingsEntry"/>.</returns>
        public static StandingsEntryViewModel Map(StandingsEntry standingsEntry)
        {
            return new StandingsEntryViewModel
            {
                TeamName = standingsEntry.TeamName,
                Points = standingsEntry.Points,
                GamesTotal = standingsEntry.GamesTotal,
                GamesWon = standingsEntry.GamesWon,
                GamesLost = standingsEntry.GamesLost,
                GamesWithScoreThreeNil = standingsEntry.GamesWithScoreThreeNil,
                GamesWithScoreThreeOne = standingsEntry.GamesWithScoreThreeOne,
                GamesWithScoreThreeTwo = standingsEntry.GamesWithScoreThreeTwo,
                GamesWithScoreTwoThree = standingsEntry.GamesWithScoreTwoThree,
                GamesWithScoreOneThree = standingsEntry.GamesWithScoreOneThree,
                GamesWithScoreNilThree = standingsEntry.GamesWithScoreNilThree,
                SetsWon = standingsEntry.SetsWon,
                SetsLost = standingsEntry.SetsLost,
                SetsRatio = standingsEntry.SetsRatio,
                BallsWon = standingsEntry.BallsWon,
                BallsLost = standingsEntry.BallsLost,
                BallsRatio = standingsEntry.BallsRatio
            };
        }

        /// <summary>
        /// Sets team positions according to their results
        /// </summary>
        /// <param name="entries">List of team standings entries</param>
        /// <returns>List of team standings entries with their position</returns>
        public static IList<StandingsEntryViewModel> SetPositions(IList<StandingsEntryViewModel> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Position = i == 0 || !EntriesHaveSamePosition(entries[i], entries[i - 1])
                    ? i + 1
                    : entries[i - 1].Position;
            }

            return entries;
        }

        /// <summary>
        /// Checks if teams entries have same position by comparing their results
        /// </summary>
        /// <param name="firstEntry">First team standings entry</param>
        /// <param name="secondEntry">Second team standings entry</param>
        /// <returns>True if teams have same position</returns>
#pragma warning disable S1244 // Want to check floating point not with exact values, but with range
        public static bool EntriesHaveSamePosition(StandingsEntryViewModel firstEntry, StandingsEntryViewModel secondEntry)
        {
            return firstEntry.Points == secondEntry.Points
                && firstEntry.SetsRatio == secondEntry.SetsRatio
                && firstEntry.BallsRatio == secondEntry.BallsRatio;
        }
    }
#pragma warning restore S1244
}
