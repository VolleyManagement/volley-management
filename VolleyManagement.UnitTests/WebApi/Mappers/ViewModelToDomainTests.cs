namespace VolleyManagement.UnitTests.WebApi.Mappers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.UserService;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UnitTests.WebApi.ViewModels;
    using VolleyManagement.WebApi.Mappers;

    /// <summary>
    /// Tests for ViewModelToDomain class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ViewModelToDomainTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map user view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_UserViewModelAsParam_MappedToDomainModel()
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
            var actual = ViewModelToDomain.Map(testUserViewModel);

            // Assert
            AssertExtensions.AreEqual<User>(expected, actual, new UserComparer());
        }
    }
}
