namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Represents an equality comparer for <see cref="GameResultViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultViewModelEqualityComparer : IEqualityComparer<GameResultViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.
        /// If both x and y are null, the method returns true.</returns>
        public bool Equals(GameResultViewModel x, GameResultViewModel y)
        {
            return x.SetScores[0].Home == y.SetScores[0].Home
                && x.SetScores[1].Home == y.SetScores[1].Home
                && x.SetScores[2].Home == y.SetScores[2].Home
                && x.SetScores[3].Home == y.SetScores[3].Home
                && x.SetScores[4].Home == y.SetScores[4].Home
                && x.SetScores[0].Away == y.SetScores[0].Away
                && x.SetScores[1].Away == y.SetScores[1].Away
                && x.SetScores[2].Away == y.SetScores[2].Away
                && x.SetScores[3].Away == y.SetScores[3].Away
                && x.SetScores[4].Away == y.SetScores[4].Away
                && x.AwayTeamId == y.AwayTeamId
                && x.SetsScore.Home == y.SetsScore.Home
                && x.SetsScore.Away == y.SetsScore.Away
                && x.HomeTeamId == y.HomeTeamId
                && x.Id == y.Id
                && x.IsTechnicalDefeat == y.IsTechnicalDefeat
                && x.TournamentId == y.TournamentId
                && x.GameDate == y.GameDate
                && x.Round == y.Round
                && x.HomeTeamName == y.HomeTeamName
                && y.AwayTeamName == y.AwayTeamName;
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="GameResultViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="GameResultViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="GameResultViewModel"/>.</returns>
        public int GetHashCode(GameResultViewModel obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.AwayTeamId);
            stringBuilder.Append(obj.AwayTeamName);
            stringBuilder.Append(obj.GameDate);
            stringBuilder.Append(obj.HomeTeamId);
            stringBuilder.Append(obj.HomeTeamName);
            stringBuilder.Append(obj.Id);
            stringBuilder.Append(obj.IsTechnicalDefeat);
            stringBuilder.Append(obj.Round);
            stringBuilder.Append(obj.SetScores);
            stringBuilder.Append(obj.SetsScore);
            stringBuilder.Append(obj.TournamentId);

            return stringBuilder.ToString().GetHashCode();
        }
    }
}
