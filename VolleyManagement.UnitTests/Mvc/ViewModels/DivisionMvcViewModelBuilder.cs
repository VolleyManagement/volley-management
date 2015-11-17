namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;

    /// <summary>
    /// Builder for test MVC contributor team view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test division view model instance
        /// </summary>
        private DivisionViewModel _divisionViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionMvcViewModelBuilder"/> class
        /// </summary>
        public DivisionMvcViewModelBuilder()
        {
            _divisionViewModel = new DivisionViewModel()
            {
                Id = 1,
                Name = "FirstDiv"
            };
        }

        /// <summary>
        /// Sets the Division view model Id
        /// </summary>
        /// <param name="id">Division view model Id</param>
        /// <returns>Division view model builder object</returns>
        public DivisionMvcViewModelBuilder WithId(int id)
        {
            _divisionViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the Division view model first name
        /// </summary>
        /// <param name="name">Division view model first name</param>
        /// <returns>Division view model builder object</returns>
        public DivisionMvcViewModelBuilder WithName(string name)
        {
            _divisionViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Builds a test Division view model
        /// </summary>
        /// <returns>test tournament view model</returns>
        public DivisionViewModel Build()
        {
            return _divisionViewModel;
        }
    }
}
