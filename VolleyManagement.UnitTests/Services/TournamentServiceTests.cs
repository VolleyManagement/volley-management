namespace VolleyManagement.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Comparers.Tournaments;
    using VolleyManagement.UnitTests.ObjectMothers.Tournaments;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [TestClass]
    public class TournamentServiceTests
    {
        /// <summary>
        /// Test for GetAll() method. The method should return existing tournaments
        /// (order is important).
        /// </summary>
        [TestMethod]
        [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules",
            Justification = "The names of the tests have their own naming conventions.")]
        public void GetAll_TournamentsExist_TournamentsReturned()
        {
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            tournamentRepositoryMock.Setup(r => r.FindAll())
                .Returns(TournamentObjectMother.CreateTournaments().AsQueryable());
            ITournamentRepository tournamentRepository = tournamentRepositoryMock.Object;
            var tournamentService = new TournamentService(tournamentRepository);

            var expectedTournaments = new List<Tournament>(
                TournamentObjectMother.CreateTournaments());
            var actualTournaments = tournamentService.GetAll().ToList();

            CollectionAssert.AreEqual(expectedTournaments, actualTournaments, new TournamentComparer());
        }
    }
}
