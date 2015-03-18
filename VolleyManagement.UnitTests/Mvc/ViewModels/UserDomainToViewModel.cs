namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.UserService;

    /// <summary>
    /// Tests for DomainToViewModel class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserDomainToViewModel
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map user domain model to view model.
        /// </summary>
        [TestMethod]

        public void Map_UserAsParam_MappedToViewModel()
        {
            //Arrange
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
            var actual = UserViewModel.Map(user);

            // Assert
            AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }
    }
}