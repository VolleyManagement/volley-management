namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.Mvc.Mappers;
    using VolleyManagement.Mvc.ViewModels.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Users;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.TournamentService;
    using VolleyManagement.UnitTests.Services.UserService;

    /// <summary>
    /// Tests for ViewModelToDomain class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map tournament view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_TournamentViewModelAsParam_MappedToDomainModel()
        {
            // Arrange
            var testViewModel = new TournamentMvcViewModelBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();
            var expected = new TournamentBuilder()
                                        .WithId(2)
                                        .WithDescription("Volley")
                                        .WithName("test tournament")
                                        .WithScheme(TournamentSchemeEnum.One)
                                        .WithSeason("2016/2017")
                                        .WithRegulationsLink("volley.dp.ua")
                                        .Build();

            // Act
            var actual = ViewModelToDomain.Map(testViewModel);

            // Assert
            AssertExtensions.AreEqual<Tournament>(expected, actual, new TournamentComparer());
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map user view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_UserViewModelAsParam_MappedToDomainModel()
        {
            // Arrange
            var testViewModel = new UserMvcViewModelBuilder()
                                        .WithId(2)
                                        .WithUserName("testLogin")
                                        .WithFullName("Test Name")
                                        .WithEmail("test2@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();
            var expected = new UserBuilder()
                                        .WithId(2)
                                        .WithUserName("testLogin")
                                        .WithFullName("Test Name")
                                        .WithEmail("test2@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();

            // Act
            var actual = ViewModelToDomain.Map(testViewModel);

            // Assert
            AssertExtensions.AreEqual<User>(expected, actual, new UserComparer());
        }
    }
}