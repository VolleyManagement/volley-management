namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;
    using Xunit;
    using Services.TournamentService;

    /// <summary>
    /// Tests for ViewModelToDomain class.
    /// </summary>   
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament view model to domain model.
        /// </summary>
        [Fact]
        public void Map_TournamentViewModelAsParam_MappedToDomainModel()
        {
            // Arrange
            var testViewModel = new TournamentMvcViewModelBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithLocation("Lviv")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithLocation("Lviv")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            Assert.Equal<Tournament>(expected, actual, new TournamentComparer());
        }
    }
}
