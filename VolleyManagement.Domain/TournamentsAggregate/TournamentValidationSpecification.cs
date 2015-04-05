namespace VolleyManagement.Domain.TournamentsAggregate
{
    using VolleyManagement.Domain.Specifications;

    /// <summary>
    /// The tournament validation specification.
    /// </summary>
    public class TournamentValidationSpecification : ISpecification<Tournament>
    {
        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool IsSatisfiedBy(Tournament entity)
        {
            // Name
            return !string.IsNullOrEmpty(entity.Name) && entity.Name.Length <= Constants.Tournament.MAX_NAME_LENGTH
                //// Description
                && (string.IsNullOrEmpty(entity.Description)
                    || entity.Description.Length <= Constants.Tournament.MAX_DESCRIPTION_LENGTH)
                //// Season
                && (string.IsNullOrEmpty(entity.Season)
                    && entity.Season.Length <= Constants.Tournament.MAX_SEASON_LENGTH)
                //// Regulation link
                && (string.IsNullOrEmpty(entity.RegulationsLink)
                      || entity.RegulationsLink.Length <= Constants.Tournament.MAX_REGULATION_LENGTH);
        }
    }
}