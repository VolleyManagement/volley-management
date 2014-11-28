// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DalToDomain.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Dal to domain mapper
// </summary>
// <author> Alex Lapin </author>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Dal.MsSql.Mappers
{
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Tournaments;
    
    /// <summary>
    /// maps DAL models to domain 
    /// </summary>
    public static class DalToDomain
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="_tournament">Tournament dal model</param>
        /// <returns>Tournament domain model</returns>
        public static Domain.Tournament MapTourament(Dal.Tournament dalTournament)
        {
            Domain.Tournament tournament = new Domain.Tournament();
            tournament.Id = dalTournament.Id;
            tournament.Name = dalTournament.Name;
            tournament.Description = dalTournament.Description;
            tournament.Season = dalTournament.Season;
            tournament.Scheme = dalTournament.Scheme;
            tournament.LinkToReglament = dalTournament.LinkToReglament;
            return tournament;
        }

    }
}
