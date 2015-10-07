namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using VolleyManagement.Domain.Players;
    using VolleyManagement.UnitTests.Services.PlayerService;

    /// <summary>
    /// View model player class test
    /// </summary>
    [TestClass]
    public class PlayerViewModelToDomainTest
    {
        /// <summary>
        /// ToDomain() method test.
        /// Does correct player view model mapped to domain model.
        /// </summary>
        [TestMethod]
        public void ToDomain_PlayerViewModel_MappedToDomain()
        {
            // Arrange
            var testViewModel = new PlayerMvcViewModelBuilder()
                .WithId(1)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .WithTeamId(1)
                .Build();

            var testDomainModel = new PlayerBuilder()
                .WithId(1)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .WithTeamId(1)
                .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            AssertExtensions.AreEqual<Player>(testDomainModel, actual, new PlayerComparer());
        }
    }
}
