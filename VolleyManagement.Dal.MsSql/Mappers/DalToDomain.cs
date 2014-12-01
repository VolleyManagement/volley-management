namespace VolleyManagement.Dal.MsSql.Mappers
{
    using Domain;

    /// <summary>
    /// Maps DAL models to domain
    /// </summary>
    public static class DalToDomain
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="dalTournament">Tournament dal model</param>
        /// <returns>Tournament domain model</returns>
        public static Domain.Tournaments.Tournament Map(Tournament dalTournament)
        {
            Domain.Tournaments.Tournament tournament = new Domain.Tournaments.Tournament();
            tournament.Id = dalTournament.Id;
            tournament.Name = dalTournament.Name;
            tournament.Description = dalTournament.Description;
            tournament.Season = dalTournament.Season;
            tournament.Scheme = dalTournament.Scheme;
            tournament.RegulationsLink = dalTournament.RegulationsLink;
            return tournament;
        }
    }
}
