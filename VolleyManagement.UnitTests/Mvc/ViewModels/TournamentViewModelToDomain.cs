namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for ViewModelToDomain class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_TournamentViewModelAsParam_MappedToDomainModel()
        {
            // Arrange
            var testViewModel = new TournamentMvcViewModelBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }
    }
}
