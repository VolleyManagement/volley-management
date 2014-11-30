namespace VolleyManagement.Dal.MsSql.Mappers
{
    using Dal = VolleyManagement.Dal.MsSql;

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
        public static Tournament MapTourament(Dal.Tournament dalTournament)
        {
            Tournament tournament = new Tournament();
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
