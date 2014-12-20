namespace VolleyManagement.UnitTests.WebApi.Mappers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UnitTests.Services.UserComparer;
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
        /// The method should map tournament view model to domain model.
        /// </summary>
        [TestMethod]
        public void Map_UserViewModelAsParam_MappedToDomainModel()
        {
            throw new NotImplementedException();
        }
    }
}
