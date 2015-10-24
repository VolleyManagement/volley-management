namespace VolleyManagement.Domain.TournamentsAggregate
{
    using VolleyManagement.Crosscutting.Contracts.Specifications;
    using VolleyManagement.Crosscutting.Specifications;

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
                && (entity.Season >= Constants.Tournament.MINIMAL_SEASON_YEAR
                    && entity.Season <= Constants.Tournament.MAXIMAL_SEASON_YEAR)
                //// Regulation link
                && (string.IsNullOrEmpty(entity.RegulationsLink)
                      || entity.RegulationsLink.Length <= Constants.Tournament.MAX_REGULATION_LENGTH);
        }
    }
}