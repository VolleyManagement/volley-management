namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

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
            var testViewModel = new TournamentViewModel
            {
                Id = 2,
                Name = "testViewModel"
            };

            // Act
            var tournament = ViewModelToDomain.Map(testViewModel);

            // Assert
            Assert.IsTrue(FieldsComparer.AreFieldsEqual(tournament, testViewModel));
        }
    }
}
