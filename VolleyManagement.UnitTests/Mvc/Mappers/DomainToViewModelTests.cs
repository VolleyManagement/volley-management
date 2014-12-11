namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UnitTests.Services.TournamentService;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

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

            //Act
            var tournamentViewModel = DomainToViewModel.Map(testTournament);

            //Assert
            Assert.IsTrue(AreFieldsEqual(testTournament,tournamentViewModel));
        }

        /// <summary>
        /// Method to check the mapping. 
        /// </summary>
        /// <param name="testTournament">domain model</param>
        /// <param name="tournamentViewModel">view model</param>
        /// <returns>true if fields are equal</returns>
        private bool AreFieldsEqual(Tournament testTournament, TournamentViewModel tournamentViewModel)
        {
            if (testTournament.Id == tournamentViewModel.Id &&
                testTournament.Name == tournamentViewModel.Name &&
                testTournament.Description == tournamentViewModel.Description &&
                testTournament.RegulationsLink == tournamentViewModel.RegulationsLink &&
                testTournament.Scheme == tournamentViewModel.Scheme &&
                testTournament.Season == tournamentViewModel.Season)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
