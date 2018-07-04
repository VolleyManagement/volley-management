namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts;
    using Moq;
    using Domain.ContributorsAggregate;
    using UI.Areas.Mvc.Controllers;
    using UI.Areas.Mvc.ViewModels.ContributorsTeam;
    using ViewModels;
    using Services.ContributorService;
    using Xunit;

    /// <summary>
    /// Tests for MVC ContributorTeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ContributorsTeamControllerTests
    {
        private Mock<IContributorTeamService> _contributorTeamServiceMock;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        public ContributorsTeamControllerTests()
        {
            _contributorTeamServiceMock = new Mock<IContributorTeamService>();
        }

        /// <summary>
        /// Test for Index method. All contributors are requested. All contributors are returned.
        /// </summary>
        [Fact]
        public void Index_GetAllContributors_AllContributorsAreReturned()
        {
            // Arrange
            var testData = MakeTestContributorTeams();
            var expected = MakeTestContributorTeamViewModels(testData);
            SetupGetAll(testData);

            var sut = BuildSUT();

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<ContributorsTeamViewModel>>(sut.Index()).ToList();

            // Assert
            Assert.Equal(expected, actual, new ContributorTeamViewModelComparer());
        }

        private List<ContributorTeam> MakeTestContributorTeams()
        {
            return new ContributorTeamServiceTestFixture().TestContributors().Build();
        }

        private List<ContributorsTeamViewModel> MakeTestContributorTeamViewModels(List<ContributorTeam> contributorTeams)
        {
            return contributorTeams.Select(ct => new ContributorTeamMvcViewModelBuilder()
                .WithId(ct.Id)
                .WithName(ct.Name)
                .WithCourseDirection(ct.CourseDirection)
                .WithContributors(ct.Contributors.ToList())
                .Build())
                .ToList();
        }

        private void SetupGetAll(List<ContributorTeam> teams)
        {
            _contributorTeamServiceMock.Setup(cts => cts.Get()).Returns(teams);
        }

        private ContributorsTeamController BuildSUT()
        {
            return new ContributorsTeamController(_contributorTeamServiceMock.Object);
        }
    }
}
