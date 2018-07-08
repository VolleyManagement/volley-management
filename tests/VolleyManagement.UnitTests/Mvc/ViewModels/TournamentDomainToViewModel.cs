namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;
    using Xunit;
    using Services.TournamentService;
    using UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Tests for DomainToViewModel class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentDomainToViewModel
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament domain model to view model.
        /// </summary>
        [Fact]
        public void Map_TournamentAsParam_MappedToViewModel()
        {
            // Arrange
            var tournament = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithLocation("Lviv")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentMvcViewModelBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithLocation("Lviv")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason(2016)
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = TournamentViewModel.Map(tournament);

            // Assert
            TournamentViewModelComparer.AssertAreEqual(expected, actual);
        }
    }
}