namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Builder for test MVC tournament view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test tournament view model instance
        /// </summary>
        private TournamentViewModel _tournamentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentMvcViewModelBuilder"/> class
        /// </summary>
        public TournamentMvcViewModelBuilder()
        {
            _tournamentViewModel = new TournamentViewModel()
            {
                Id = 1,
                Name = "Name",
                Description = "Description 1",
                Season = "2014/2015",
                Scheme = TournamentSchemeEnum.Two,
                RegulationsLink = "http://default.com"
            };
        }

        /// <summary>
        /// Sets id of test tournament view model
        /// </summary>
        /// <param name="id">Id for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithId(int id)
        {
            _tournamentViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament view model
        /// </summary>
        /// <param name="name">Name for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithName(string name)
        {
            _tournamentViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament view model
        /// </summary>
        /// <param name="description">Description for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithDescription(string description)
        {
            _tournamentViewModel.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament view model
        /// </summary>
        /// <param name="scheme">Scheme for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            _tournamentViewModel.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament view model
        /// </summary>
        /// <param name="season">Season for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithSeason(string season)
        {
            _tournamentViewModel.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament view model
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithRegulationsLink(string regulationsLink)
        {
            _tournamentViewModel.RegulationsLink = regulationsLink;
            return this;
        }

        /// <summary>
        /// Builds test tournament view model
        /// </summary>
        /// <returns>test tournament view model</returns>
        public TournamentViewModel Build()
        {
            return _tournamentViewModel;
        }
    }
}
