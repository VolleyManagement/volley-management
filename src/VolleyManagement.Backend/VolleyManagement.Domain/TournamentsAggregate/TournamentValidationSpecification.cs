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
            if (IsSatisfiedByName(entity))
            {
                return false;
            }
            else if (IsSatisfiedByLocation(entity))
            {
                return false;
            }
            else if (IsSatisfiedByDescription(entity))
            {
                return false;
            }
            else if (IsSatisfiedBySeason(entity))
            {
                return false;
            }
            else if (IsSatisfiedByRegulationsLink(entity))
            {
                return false;
            }
            return true;
        }

        private static bool IsSatisfiedByName(Tournament entity)
        {
            return (string.IsNullOrEmpty(entity.Name) || entity.Name.Length >= Constants.Tournament.MAX_NAME_LENGTH);
        }

        private static bool IsSatisfiedByDescription(Tournament entity)
        {           
            return (!string.IsNullOrEmpty(entity.Description)
                    && entity.Description.Length >= Constants.Tournament.MAX_DESCRIPTION_LENGTH);
        }

        private static bool IsSatisfiedByLocation(Tournament entity)
        {
            return (!string.IsNullOrEmpty(entity.Location)
                    && entity.Location.Length >= Constants.Tournament.MAX_LOCATION_LENGTH);
        }

        private static bool IsSatisfiedBySeason(Tournament entity)
        {
            return (entity.Season <= Constants.Tournament.MINIMAL_SEASON_YEAR
                && entity.Season >= Constants.Tournament.MAXIMAL_SEASON_YEAR);
        }

        private static bool IsSatisfiedByRegulationsLink(Tournament entity)
        {
            return (!string.IsNullOrEmpty(entity.RegulationsLink)
                && entity.RegulationsLink.Length >= Constants.Tournament.MAX_REGULATION_LENGTH);
        }
    }
}
