namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Comparers;
    using FluentAssertions;
    using Xunit;
    using Services.GameService;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultViewModelComparer : IComparer<GameResultViewModel>, IComparer, IEqualityComparer<GameResultViewModel>
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(GameResultViewModel x, GameResultViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            var firstGameResult = x as GameResultViewModel;
            var secondGameResult = y as GameResultViewModel;

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
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(GameResultViewModel x, GameResultViewModel y)
        {
            y.Id.Should().Be(x.Id, "Name should be equal.");
            y.TournamentId.Should().Be(x.TournamentId, "Name should be equal.");

            y.AwayTeamId.Should().Be(x.AwayTeamId, "AwayTeamId should be equal.");
            y.HomeTeamId.Should().Be(x.HomeTeamId, "HomeTeamId should be equal.");
            y.AwayTeamName.Should().Be(x.AwayTeamName, "AwayTeamName should be equal.");
            y.HomeTeamName.Should().Be(x.HomeTeamName, "HomeTeamName should be equal.");

            y.GameDate.Should().Be(x.GameDate, "GameDate should be equal.");
            y.Round.Should().Be(x.Round, "Round should be equal.");
            y.DisplayGameNumber.Should().Be(x.DisplayGameNumber, "DisplayGameNumber should be equal.");
            y.IsTechnicalDefeat.Should().Be(x.IsTechnicalDefeat, "IsTechnicalDefeat should be equal.");

            y.HasPenalty.Should().Be(x.HasPenalty, "HasPenalty should be equal.");
            y.IsHomeTeamPenalty.Should().Be(x.IsHomeTeamPenalty, "IsHomeTeamPenalty should be equal.");
            y.PenaltyAmount.Should().Be(x.PenaltyAmount, "PenaltyAmount should be equal.");
            y.PenaltyDescrition.Should().Be(x.PenaltyDescrition, "PenaltyDescrition should be equal.");

            y.UrlToGameVideo.Should().Be(x.UrlToGameVideo, "UrlToGameVideo should be equal.");

            y.AllowEditTotalScore.Should().Be(x.AllowEditTotalScore, "AllowEditResult should be equal.");

            ScoreViewModelComparer.AssertEqual(x.GameScore, y.GameScore);
            for (var i = 0; i < 5; i++)
            {
                ScoreViewModelComparer.AssertEqual(x.SetScores[i], y.SetScores[i], $"[Set:{i + 1}]");
            }

            return true;
        }

        public bool Equals(GameResultViewModel x, GameResultViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(GameResultViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}