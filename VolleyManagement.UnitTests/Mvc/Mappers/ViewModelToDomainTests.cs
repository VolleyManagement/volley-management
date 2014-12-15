namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Mvc.ViewModels;

    /// <summary>
    /// Tests for ViewModelToDomain class.
    /// </summary>
    [TestClass]
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
                                        .WithName("testViewModel")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var tournament = ViewModelToDomain.Map(testViewModel);

            // Assert
            Assert.IsTrue(FieldsComparer.AreFieldsEqual(tournament, testViewModel));
        }
    }
}
