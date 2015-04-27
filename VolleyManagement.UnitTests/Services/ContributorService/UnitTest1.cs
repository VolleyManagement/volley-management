using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Ninject;
using VolleyManagement.Dal.MsSql.Infrastructure;
using VolleyManagement.Dal.Contracts;

namespace VolleyManagement.UnitTests.Services.ContributorService
{
    [TestClass]
    public class ContributorsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var kernel = new StandardKernel(new NinjectDataAccessModule(null));

            var repo = kernel.Get<IContributorTeamRepository>();

            var data = repo.Find().ToList();

            Assert.IsNotNull(data);
        }
    }
}
