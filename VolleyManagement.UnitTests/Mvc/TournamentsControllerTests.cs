namespace VolleyManagement.UnitTests.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc;
    using VolleyManagement.Mvc.Controllers;

    /// <summary>
    /// TournamentsController test class
    /// </summary>
    [TestClass]
    public class TournamentsControllerTests
    {
        /// <summary>
        /// Test method
        /// </summary>
        [TestMethod]
        public void TestDetailsView()
        {
            var tournamentServiceMock = new Mock<ITournamentService>();
            int id = 1;
            tournamentServiceMock.Setup(ts => ts.FindById(id))
                .Returns(new Tournament { Id = id });

            var tournamentsController = new TournamentsController(tournamentServiceMock.Object);

            var result = tournamentsController.Details(2) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }
    }
}
