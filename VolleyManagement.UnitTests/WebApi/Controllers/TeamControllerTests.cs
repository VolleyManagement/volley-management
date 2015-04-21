namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Http.Results;
    using System.Web.OData.Results;

    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;
    using VolleyManagement.UnitTests.Services.TeamService;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for TeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly TeamServiceTestFixture _testFixture = new TeamServiceTestFixture();

        /// <summary>
        /// Team Service Mock
        /// </summary>
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITeamService>()
                   .ToConstant(_teamServiceMock.Object);
        }
    }
}
