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
    using VolleyManagement.UnitTests.Builders.Tournaments;
    using VolleyManagement.UnitTests.Comparers.Tournaments;

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
            var tournamentService = new TournamentService(CreateMockTournamentRepository());

            var expectedTournaments = new TournamentsAppServiceFixture()
                                            .TestTournaments()
                                            .Build();

            var actualTournaments = tournamentService.GetAll().OrderBy(t => t.Id).ToList();

            Assert.IsTrue(expectedTournaments.SequenceEqual(actualTournaments, new TournamentComparer()));
        }

        /// <summary>
        /// Creates mock of ITournamentRepository
        /// </summary>
        /// <returns>Tournament repository mock</returns>
        private ITournamentRepository CreateMockTournamentRepository()
        {
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            mockTournamentRepository.Setup(r => r.FindAll())
                .Returns(new TournamentsAppServiceFixture()
                                .TestTournaments()
                                .Build()
                                .AsQueryable());
            return mockTournamentRepository.Object;
        }
    }
}
