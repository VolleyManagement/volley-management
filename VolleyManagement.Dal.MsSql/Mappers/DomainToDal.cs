namespace VolleyManagement.Dal.MsSql.Mappers
{
    using Domain;

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
        public static Tournament Map(Domain.Tournaments.Tournament domainTournament)
        {
            Tournament tournament = new Tournament();
            tournament.Id = domainTournament.Id;
            tournament.Name = domainTournament.Name;
            tournament.Season = domainTournament.Season;
            tournament.Description = domainTournament.Description;
            tournament.Scheme = domainTournament.Scheme;
            tournament.RegulationsLink = domainTournament.RegulationsLink;
            return tournament;
        }
    }
}
