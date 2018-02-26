namespace VolleyManagement.Domain.TournamentsAggregate
{
    using Crosscutting.Contracts.Specifications;

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
            if (string.IsNullOrEmpty(entity.Name) || entity.Name.Length >= Constants.Tournament.MAX_NAME_LENGTH)
            {
                return false;
            }
            //// Description
            if (!string.IsNullOrEmpty(entity.Description)
                && entity.Description.Length >= Constants.Tournament.MAX_DESCRIPTION_LENGTH)
            {
                return false;
            }
            //// Season
            if (entity.Season <= Constants.Tournament.MINIMAL_SEASON_YEAR
                && entity.Season >= Constants.Tournament.MAXIMAL_SEASON_YEAR)
            {
                return false;
            }
            //// Regulation link
            if (!string.IsNullOrEmpty(entity.RegulationsLink)
                  && entity.RegulationsLink.Length >= Constants.Tournament.MAX_REGULATION_LENGTH)
            {
                return false;
            }

            return true;
        }
    }
}
