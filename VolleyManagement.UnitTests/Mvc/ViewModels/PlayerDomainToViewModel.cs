namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UnitTests.Services.PlayerService;

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
                .Build();

            var testDomainModel = new PlayerBuilder()
                .WithId(1)
                .WithFirstName("FirstName")
                .WithLastName("LastName")
                .WithBirthYear(1983)
                .WithHeight(186)
                .WithWeight(95)
                .Build();

            // Act
            var actual = PlayerViewModel.Map(testDomainModel);

            // Assert
            AssertExtensions.AreEqual<PlayerViewModel>(testViewModel, actual, new PlayerViewModelComparer());
        }
    }
}
