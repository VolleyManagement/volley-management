namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.UnitTests.Services.TournamentService;

    /// <summary>
    /// Tests for DomainToViewModel class.
    /// </summary>
    [TestClass]
    public class DomainToViewModelTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_TournamentAsParam_MappedToViewModel()
        {
            // Arrange
            var testTournament = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("Test Tournament")
                                        .Build();

            // Act
            var tournamentViewModel = DomainToViewModel.Map(testTournament);

            // Assert
            Assert.IsTrue(FieldsComparer.AreFieldsEqual(testTournament, tournamentViewModel));
        }
    }
}
