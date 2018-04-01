namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.PlayerService;
    using UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// View model player class test
    /// </summary>
    [TestClass]
    public class PlayerDomainToViewModel
    {
        /// <summary>
        /// Map() method test.
        /// Does correct a player domain model mapped to a view model.
        /// </summary>
        [TestMethod]
        public void Map_DomainPlayerAsParam_MappedToViewModel()
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

            var testDomainModel = new PlayerBuilder(1, "FirstName", "LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();

            // Act
            var actual = PlayerViewModel.Map(testDomainModel);

            // Assert
            TestHelper.AreEqual(testViewModel, actual, new PlayerViewModelComparer());
        }
    }
}
