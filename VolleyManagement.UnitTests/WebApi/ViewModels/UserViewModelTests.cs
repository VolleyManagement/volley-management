namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Users;
    using VolleyManagement.UnitTests.Services.UserService;

    /// <summary>
    /// UserViewModel class tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserViewModelTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map user domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_UserDomainModelAsParam_MappedToViewModel()
        {
            // Arrange
            var testUser = new UserBuilder()
                            .WithId(2)
                            .WithUserName("UserLogin")
                            .WithFullName("Second User")
                            .WithEmail("seconduser@gmail.com")
                            .WithPassword("abc222")
                            .WithCellPhone("0503222233")
                            .Build();
            var expected = new UserViewModelBuilder()
                                        .WithId(2)
                                        .WithUserName("UserLogin")
                                        .WithFullName("Second User")
                                        .WithEmail("seconduser@gmail.com")
                                        .WithPassword(string.Empty)
                                        .WithCellPhone("0503222233")
                                        .Build();

            // Act
            var actual = UserViewModel.Map(testUser);

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }

        /// <summary>
        /// Test for ToDomain() method.
        /// The method should map user view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_UserViewModel_MappedToDomainModel()
        {
            // Arrange
            var testUserViewModel = new UserViewModelBuilder()
                                        .WithId(2)
                                        .WithUserName("UserVolley")
                                        .WithFullName("Second User")
                                        .WithEmail("seconduser@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();
            var expected = new UserBuilder()
                                        .WithId(2)
                                        .WithUserName("UserVolley")
                                        .WithFullName("Second User")
                                        .WithEmail("seconduser@gmail.com")
                                        .WithPassword("abc222")
                                        .WithCellPhone("0500000002")
                                        .Build();

            // Act
            var actual = testUserViewModel.ToDomain();

            // Assert
            AssertExtensions.AreEqual<User>(expected, actual, new UserComparer());
        }
    }
}
