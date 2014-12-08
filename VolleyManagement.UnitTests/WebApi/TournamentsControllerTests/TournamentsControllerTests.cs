namespace VolleyManagement.UnitTests.WebApi.TournamentsControllerTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Test TournamentsController.
    /// </summary>
    [TestClass]
    public class TournamentsControllerTests
    {
        /// <summary>
        /// Tournaments Repository Mock
        /// </summary>
        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock = new Mock<ITournamentRepository>();

        /// <summary>
        /// Tournaments Service Mock
        /// </summary>
        private readonly Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();

        /// <summary>
        /// Test Post method.
        /// </summary>
        [TestMethod]
        public void Post_NewTournament_TournamentAdded()
        {
            Tournament t = new Tournament { Id = 1, Name = "Tournament 1" };
            List<Tournament> tournaments = new List<Tournament>();
            _tournamentServiceMock.Setup(ts => ts.Create(It.IsAny<Tournament>()))
                .Callback((Tournament tournament) =>
            {
                _tournamentRepositoryMock.Setup(r => r.Add(It.IsAny<Tournament>()))
                    .Callback((Tournament tourn) =>
                    {
                        tournaments.Add(tourn);
                    });
            }
                );
            ITournamentService service = _tournamentServiceMock.Object;
            ITournamentRepository repository = _tournamentRepositoryMock.Object;
            service.Create(t);
            CollectionAssert.Contains(tournaments, t);
        }
    }
}
