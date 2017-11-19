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
            return new GameResultViewModelComparer().Compare(x, y) == 0;
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
