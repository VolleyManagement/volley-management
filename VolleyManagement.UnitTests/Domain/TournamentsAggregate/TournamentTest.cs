namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Tests for <see cref="Tournament"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentTest
    {
        /// <summary>
        /// Test for State method. Return Not started.
        /// </summary>
        [TestMethod]
        public void State_Returns_NotStartedState()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithNotStartedState().Build();

            // Act
            var state = tournament.State;

            // Assert
            Assert.AreEqual(TournamentStateEnum.NotStarted, state);
        }

        /// <summary>
        /// Test for State method. Return Not current.
        /// </summary>
        [TestMethod]
        public void State_Returns_CurrentState()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithCurrentState().Build();

            // Act
            var state = tournament.State;

            // Assert
            Assert.AreEqual(TournamentStateEnum.Current, state);
        }

        /// <summary>
        /// Test for State method. Return Not upcoming.
        /// </summary>
        [TestMethod]
        public void State_Returns_UpcomingState()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithUpcomingState().Build();

            // Act
            var state = tournament.State;

            // Assert
            Assert.AreEqual(TournamentStateEnum.Upcoming, state);
        }

        /// <summary>
        /// Test for State method. Return Not finished.
        /// </summary>
        [TestMethod]
        public void State_Returns_FinishedState()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithFinishedState().Build();

            // Act
            var state = tournament.State;

            // Assert
            Assert.AreEqual(TournamentStateEnum.Finished, state);
        }
    }
}