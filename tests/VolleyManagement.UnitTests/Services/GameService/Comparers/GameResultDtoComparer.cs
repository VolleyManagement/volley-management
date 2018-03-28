using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VolleyManagement.Domain.GamesAggregate;

namespace VolleyManagement.UnitTests.Services.GameService
{
    /// <summary>
    /// Represents a comparer for <see cref="GameResultDto"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultDtoComparer : IComparer<GameResultDto>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultDto"/> x and y.</returns>
        public int Compare(GameResultDto x, GameResultDto y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="GameResultDto"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="GameResultDto"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            var firstGameResult = x as GameResultDto;
            var secondGameResult = y as GameResultDto;

            if (firstGameResult == null)
            {
                return -1;
            }
            else if (secondGameResult == null)
            {
                return 1;
            }

            return Compare(firstGameResult, secondGameResult);
        }

        /// <summary>
        /// Finds out whether two <see cref="GameResultDto"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResultDto"/> objects are equal.</returns>
        internal bool AreEqual(GameResultDto x, GameResultDto y)
        {
            Assert.AreEqual(x.Id, y.Id, $"[Id:{x.Id}] Id should be equal.");
            Assert.AreEqual(x.TournamentId, y.TournamentId, $"[Id:{x.Id}] TournamentId should be equal.");
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, $"[Id:{x.Id}] HomeTeamId should be equal.");
            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, $"[Id:{x.Id}] AwayTeamId should be equal.");
            Assert.AreEqual(x.HomeTeamName, y.HomeTeamName, $"[Id:{x.Id}] HomeTeamName should be equal.");
            Assert.AreEqual(x.AwayTeamName, y.AwayTeamName, $"[Id:{x.Id}] AwayTeamName should be equal.");
            Assert.AreEqual(x.Result.GameScore.Home, y.Result.GameScore.Home,
                $"[Id:{x.Id}] Result.GameScore.Home should be equal.");
            Assert.AreEqual(x.Result.GameScore.Away, y.Result.GameScore.Away,
                $"[Id:{x.Id}] Result.GameScore.Away should be equal.");
            Assert.AreEqual(x.Result.GameScore.IsTechnicalDefeat, y.Result.GameScore.IsTechnicalDefeat,
                $"[Id:{x.Id}] IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.Result.SetScores[0].Home, y.Result.SetScores[0].Home,
                $"[Id:{x.Id}] SetScores[0].Home should be equal.");
            Assert.AreEqual(x.Result.SetScores[0].Away, y.Result.SetScores[0].Away,
                $"[Id:{x.Id}] SetScores[0].Away should be equal.");
            Assert.AreEqual(x.Result.SetScores[0].IsTechnicalDefeat, y.Result.SetScores[0].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[0].IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.Result.SetScores[1].Home, y.Result.SetScores[1].Home,
                $"[Id:{x.Id}] SetScores[1].Home should be equal.");
            Assert.AreEqual(x.Result.SetScores[1].Away, y.Result.SetScores[1].Away,
                $"[Id:{x.Id}] SetScores[1].Away should be equal.");
            Assert.AreEqual(x.Result.SetScores[1].IsTechnicalDefeat, y.Result.SetScores[1].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[1].IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.Result.SetScores[2].Home, y.Result.SetScores[2].Home,
                $"[Id:{x.Id}] SetScores[2].Home should be equal.");
            Assert.AreEqual(x.Result.SetScores[2].Away, y.Result.SetScores[2].Away,
                $"[Id:{x.Id}] SetScores[2].Away should be equal.");
            Assert.AreEqual(x.Result.SetScores[2].IsTechnicalDefeat, y.Result.SetScores[2].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[2].IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.Result.SetScores[3].Home, y.Result.SetScores[3].Home,
                $"[Id:{x.Id}] SetScores[3].Home should be equal.");
            Assert.AreEqual(x.Result.SetScores[3].Away, y.Result.SetScores[3].Away,
                $"[Id:{x.Id}] SetScores[3].Away should be equal.");
            Assert.AreEqual(x.Result.SetScores[3].IsTechnicalDefeat, y.Result.SetScores[3].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[3].IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.Result.SetScores[4].Home, y.Result.SetScores[4].Home,
                $"[Id:{x.Id}] SetScores[4].Home should be equal.");
            Assert.AreEqual(x.Result.SetScores[4].Away, y.Result.SetScores[4].Away,
                $"[Id:{x.Id}] SetScores[4].Away should be equal.");
            Assert.AreEqual(x.Result.SetScores[4].IsTechnicalDefeat, y.Result.SetScores[4].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[4].IsTechnicalDefeat should be equal.");
            Assert.AreEqual(x.GameDate, y.GameDate, $"[Id:{x.Id}] GameDate should be equal.");
            Assert.AreEqual(x.Round, y.Round, $"[Id:{x.Id}] Round number should be equal.");
            Assert.AreEqual(x.GameNumber, y.GameNumber, $"[Id:{x.Id}] GameNumber should be equal.");
            Assert.AreEqual(x.AllowEditResult,y.AllowEditResult, $"[Id:{x.Id}] AllowEditResult should be equal.");
            return true;
        }
    }
}