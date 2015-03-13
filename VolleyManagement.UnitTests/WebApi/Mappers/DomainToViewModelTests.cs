namespace VolleyManagement.UnitTests.WebApi.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services.UserService;

    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for DomainToViewModel class.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DomainToViewModelTests
    {
        /// <summary>
        /// Test for Map() method.
        /// The method should map user domain model to view model.
        /// </summary>
        [TestMethod]
        [Ignore]// BUG: FIX ASAP
        public void Map_UserDomainModelAsParam_MappedToViewModel()
        {
            // Arrange
            //var testUser = new UserBuilder()
            //                .WithId(2)
            //                .WithUserName("UserLogin")
            //                .WithFullName("Second User")
            //                .WithEmail("seconduser@gmail.com")
            //                .WithPassword("abc222")
            //                .WithCellPhone("0503222233")
            //                .Build();
            //var expected = new UserViewModelBuilder()
            //                            .WithId(2)
            //                            .WithUserName("UserLogin")
            //                            .WithFullName("Second User")
            //                            .WithEmail("seconduser@gmail.com")
            //                            .WithPassword(string.Empty)
            //                            .WithCellPhone("0503222233")
            //                            .Build();

            //// Act
            //var actual = DomainToViewModel.Map(testUser);

            //// Assert
            //AssertExtensions.AreEqual<UserViewModel>(expected, actual, new UserViewModelComparer());
        }
    }
}
