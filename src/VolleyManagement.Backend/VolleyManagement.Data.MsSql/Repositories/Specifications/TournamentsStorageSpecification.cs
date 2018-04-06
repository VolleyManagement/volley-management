﻿namespace VolleyManagement.Data.MsSql.Repositories.Specifications
{
    using Crosscutting.Contracts.Specifications;
    using Crosscutting.Specifications;
    using Entities;

    /// <summary>
    /// Provides specification for storing Tournaments in the DB
    /// </summary>
    public class TournamentsStorageSpecification : ISpecification<TournamentEntity>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public bool IsSatisfiedBy(TournamentEntity entity)
        {
            var name = new ExpressionSpecification<TournamentEntity>(
                                t => !string.IsNullOrEmpty(t.Name)
                                  && t.Name.Length <= ValidationConstants.Tournament.MAX_NAME_LENGTH);
            var description = new ExpressionSpecification<TournamentEntity>(
                                t => t.Description == null
                                  || t.Description.Length <= ValidationConstants.Tournament.MAX_DESCRIPTION_LENGTH);
            var city = new ExpressionSpecification<TournamentEntity>(
                t => t.City == null
                     || t.City.Length <= ValidationConstants.Tournament.MAX_CITY_LENGTH);
            var link = new ExpressionSpecification<TournamentEntity>(
                                t => t.RegulationsLink == null
                                  || t.RegulationsLink.Length <= ValidationConstants.Tournament.MAX_URL_LENGTH);

            return name.And(description)
                       .And(city)
                       .And(link)
                       .IsSatisfiedBy(entity);
        }
    }
}