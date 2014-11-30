namespace VolleyManagement.Dal.MsSql.Mappers
{
    using Dal = VolleyManagement.Dal.MsSql;

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
        public static Dal.Tournament Map(Tournament domainTournament)
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
