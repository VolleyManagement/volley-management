namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Builder for test tournaments
    /// </summary>
    internal class TournamentBuilder
    {
        /// <summary>
        /// Holds test tournament instance
        /// </summary>
        private Tournament _tournament = new Tournament();

        /// <summary>
        /// Sets id of test tournament
        /// </summary>
        /// <param name="id">Id for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithId(int id)
        {
            this._tournament.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament
        /// </summary>
        /// <param name="name">Name for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithName(string name)
        {
            this._tournament.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament
        /// </summary>
        /// <param name="description">Description for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithDescription(string description)
        {
            this._tournament.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament
        /// </summary>
        /// <param name="scheme">Scheme for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            this._tournament.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament
        /// </summary>
        /// <param name="season">Season for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithSeason(string season)
        {
            this._tournament.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithRegulationsLink(string regulationsLink)
        {
            this._tournament.RegulationsLink = regulationsLink;
            return this;
        }

        /// <summary>
        /// Builds test tournament
        /// </summary>
        /// <returns>Test tournament</returns>
        public Tournament Build()
        {
            return this._tournament;
        }
    }
}
