namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UnitTests.Services.PlayerService;

    /// <summary>
    /// View model player name class test
    /// </summary>
    [TestClass]
    public class PlayerNameViewModelToDomain
    {
        /// <summary>
        /// ToDomain() method test.
        /// Does correct player name view model mapped to domain model.
        /// </summary>
        [TestMethod]
        public void ToDomain_PlayerNameViewModel_MappedToDomain()
        {
            // Arrange
            var testViewModel = new PlayerNameMvcViewModelBuilder()
                .WithId(1)
                .Build();

            var testDomainModel = new PlayerBuilder()
               .WithId(1)
               .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            Assert.AreEqual(testDomainModel.Id, actual.Id);
        }
    }
}
