using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using VolleyManagement.Domain.GamesAggregate;

namespace VolleyManagement.UnitTests.Services.GameService
{
    /// <summary>
    /// Represents a comparer for <see cref="GameResultDto"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultDtoComparer : IComparer<GameResultDto>, IComparer, IEqualityComparer<GameResultDto>
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

        public bool Equals(GameResultDto x, GameResultDto y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(GameResultDto obj)
        {
            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two <see cref="GameResultDto"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="GameResultDto"/> objects are equal.</returns>
        internal bool AreEqual(GameResultDto x, GameResultDto y)
        {
            y.Id.Should().Be(x.Id, $"[Id:{x.Id}] Id should be equal.");
            y.TournamentId.Should().Be(x.TournamentId, $"[Id:{x.Id}] TournamentId should be equal.");
            y.HomeTeamId.Should().Be(x.HomeTeamId, $"[Id:{x.Id}] HomeTeamId should be equal.");
            y.AwayTeamId.Should().Be(x.AwayTeamId, $"[Id:{x.Id}] AwayTeamId should be equal.");
            y.HomeTeamName.Should().Be(x.HomeTeamName, $"[Id:{x.Id}] HomeTeamName should be equal.");
            y.AwayTeamName.Should().Be(x.AwayTeamName, $"[Id:{x.Id}] AwayTeamName should be equal.");
            y.Result.GameScore.Home.Should().Be(x.Result.GameScore.Home, $"[Id:{x.Id}] Result.GameScore.Home should be equal.");
            y.Result.GameScore.Away.Should().Be(x.Result.GameScore.Away, $"[Id:{x.Id}] Result.GameScore.Away should be equal.");
            y.Result.GameScore.IsTechnicalDefeat.Should().Be(x.Result.GameScore.IsTechnicalDefeat, $"[Id:{x.Id}] IsTechnicalDefeat should be equal.");
            y.Result.SetScores[0].Home.Should().Be(x.Result.SetScores[0].Home, $"[Id:{x.Id}] SetScores[0].Home should be equal.");
            y.Result.SetScores[0].Away.Should().Be(x.Result.SetScores[0].Away, $"[Id:{x.Id}] SetScores[0].Away should be equal.");
            y.Result.SetScores[0].IsTechnicalDefeat.Should().Be(x.Result.SetScores[0].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[0].IsTechnicalDefeat should be equal.");
            y.Result.SetScores[1].Home.Should().Be(x.Result.SetScores[1].Home,
                $"[Id:{x.Id}] SetScores[1].Home should be equal.");
            y.Result.SetScores[1].Away.Should().Be(x.Result.SetScores[1].Away, $"[Id:{x.Id}] SetScores[1].Away should be equal.");
            y.Result.SetScores[1].IsTechnicalDefeat.Should().Be(x.Result.SetScores[1].IsTechnicalDefeat,
                $"[Id:{x.Id}] SetScores[1].IsTechnicalDefeat should be equal.");
            y.Result.SetScores[2].Home.Should().Be(x.Result.SetScores[2].Home, $"[Id:{x.Id}] SetScores[2].Home should be equal.");
            y.Result.SetScores[2].Away.Should().Be(x.Result.SetScores[2].Away, $"[Id:{x.Id}] SetScores[2].Away should be equal.");
            y.Result.SetScores[2].IsTechnicalDefeat.Should().Be(x.Result.SetScores[2].IsTechnicalDefeat, $"[Id:{x.Id}] SetScores[2].IsTechnicalDefeat should be equal.");
            y.Result.SetScores[3].Home.Should().Be(x.Result.SetScores[3].Home, $"[Id:{x.Id}] SetScores[3].Home should be equal.");
            y.Result.SetScores[3].Away.Should().Be(x.Result.SetScores[3].Away, $"[Id:{x.Id}] SetScores[3].Away should be equal.");
            y.Result.SetScores[3].IsTechnicalDefeat.Should().Be(x.Result.SetScores[3].IsTechnicalDefeat, $"[Id:{x.Id}] SetScores[3].IsTechnicalDefeat should be equal.");
            y.Result.SetScores[4].Home.Should().Be(x.Result.SetScores[4].Home, $"[Id:{x.Id}] SetScores[4].Home should be equal.");
            y.Result.SetScores[4].Away.Should().Be(x.Result.SetScores[4].Away, $"[Id:{x.Id}] SetScores[4].Away should be equal.");
            y.Result.SetScores[4].IsTechnicalDefeat.Should().Be(x.Result.SetScores[4].IsTechnicalDefeat, $"[Id:{x.Id}] SetScores[4].IsTechnicalDefeat should be equal.");
            y.GameDate.Should().Be(x.GameDate, $"[Id:{x.Id}] GameDate should be equal.");
            y.Round.Should().Be(x.Round, $"[Id:{x.Id}] Round number should be equal.");
            y.GameNumber.Should().Be(x.GameNumber, $"[Id:{x.Id}] GameNumber should be equal.");
            y.AllowEditTotalScore.Should().Be(x.AllowEditTotalScore, $"[Id:{x.Id}] AllowEditResult should be equal.");
            return true;
        }
    }
}