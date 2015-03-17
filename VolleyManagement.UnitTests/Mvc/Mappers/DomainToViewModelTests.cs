namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TournamentService;
    using VolleyManagement.UnitTests.Services.UserService;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;

    /// <summary>
    /// Tests for DomainToViewModel class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
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
            var tournament = new TournamentBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentMvcViewModelBuilder()
                                        .WithId(1)
                                        .WithName("test")
                                        .WithDescription("Volley")
                                        .WithScheme(TournamentSchemeEnum.Two)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = DomainToViewModel.Map(tournament);

            // Assert
            AssertExtensions.AreEqual<TournamentViewModel>(expected, actual, new TournamentViewModelComparer());
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map user domain model to view model.
        /// </summary>
        [TestMethod]
    
        public void Map_UserAsParam_MappedToViewModel()
        {
           //  Arrange
            var user = new UserBuilder()
                                        .WithId(2)
                                        .WithUserName("testLogin")
                                        .WithFullName("Test Name")
                                        .WithEmail("test2@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();
            var expected = new UserMvcViewModelBuilder()
                                        .WithId(2)
                                        .WithUserName("testLogin")
                                        .WithFullName("Test Name")
                                        .WithEmail("test2@gmail.com")
                                        .WithPassword(string.Empty)
                                        .WithCellPhone("0500000002")
                                        .Build();

            // Act
            var actual = DomainToViewModel.Map(user);

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }
    }
}