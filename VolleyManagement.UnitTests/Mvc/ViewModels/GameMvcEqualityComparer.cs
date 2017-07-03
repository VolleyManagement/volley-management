namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Domain.GamesAggregate;

    /// <summary>
    /// Comparer for game objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameMvcEqualityComparer : IEqualityComparer<Game>
    {
          /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.
        /// If both x and y are null, the method returns true.</returns>
        public bool Equals(Game x, Game y)
        {
            return x.AwayTeamId == y.AwayTeamId
                && x.HomeTeamId == y.HomeTeamId
                && x.Id == y.Id
                && x.TournamentId == y.TournamentId
                && x.GameDate == y.GameDate
                && x.Round == y.Round
                && x.AwayTeamId == y.AwayTeamId
                && x.Result == x.Result;
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="GameResultViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="GameResultViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="GameResultViewModel"/>.</returns>
        public int GetHashCode(Game obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.AwayTeamId);
            stringBuilder.Append(obj.GameDate);
            stringBuilder.Append(obj.HomeTeamId);
            stringBuilder.Append(obj.Id);
            stringBuilder.Append(obj.Round);
            stringBuilder.Append(obj.TournamentId);
            stringBuilder.Append(obj.Result);

            return stringBuilder.ToString().GetHashCode();
        }
    }
}
