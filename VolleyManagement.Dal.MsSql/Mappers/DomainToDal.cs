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
        /// <param name="domainTournament">Tournament Domain model</param>
        /// <returns>Tournament Dal model</returns>
        public static Dal.Tournament Map(Domain.Tournament domainTournament)
        {
            Dal.Tournament tournament = new Dal.Tournament();
            tournament.Id = domainTournament.Id;
            tournament.Name = domainTournament.Name;
            tournament.Season = domainTournament.Season;
            tournament.Description = domainTournament.Description;
            tournament.Scheme = domainTournament.Scheme;
            tournament.LinkToReglament = domainTournament.LinkToReglament;
            return tournament;
        }
    }
}
