// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainToDal.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines IRepository contract.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Dal.MsSql.Mappers
{
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Tournaments;
   
    /// <summary>
    /// Maps Domain models to Dal.
    /// </summary>
    public static class DomainToDal
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="_tournament">Tournament Domain model</param>
        /// <returns>Tournament Dal model</returns>
        public static Dal.Tournament GetTourament(Domain.Tournament _tournament)
        {
            Dal.Tournament tournament = new Dal.Tournament();
            tournament.Id = _tournament.Id;
            tournament.Name = _tournament.Name;
            tournament.Season = _tournament.Season;
            return tournament;
        }
    }
}
