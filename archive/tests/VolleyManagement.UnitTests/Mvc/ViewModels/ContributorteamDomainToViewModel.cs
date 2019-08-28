namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using Domain.ContributorsAggregate;
    using Xunit;
    using Services.ContributorService;
    using UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// View model contributor team class test
    /// </summary>   
    public class ContributorteamDomainToViewModel
    {
        /// <summary>
        /// Map() method test.
        /// Does correct a contributor team domain model mapped to a view model.
        /// </summary>
        [Fact]
        public void Map_DomainContributorTeamAsParam_MappedToViewModel()
        {
            var contributors = new List<Contributor>
                {
                    new Contributor
                    {
                        Id = 1,
                        Name = "FirstNameA",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 2,
                        Name = "FirstNameB",
                        ContributorTeamId = 1
                    },
                    new Contributor
                    {
                        Id = 3,
                        Name = "FirstNameC",
                        ContributorTeamId = 1
                    }
                };

            // Arrange
            var testViewModel = new ContributorTeamMvcViewModelBuilder()
                .WithId(1)
                .WithName("FirstName")
                .WithCourseDirection("Course")
                .WithContributors(contributors)
                .Build();

            var testDomainModel = new ContributorTeamBuilder()
                .WithId(1)
                .WithName("FirstName")
                .WithcourseDirection("Course")
                .Withcontributors(contributors)
                .Build();

            // Act
            var actual = ContributorsTeamViewModel.Map(testDomainModel);

            // Assert
            Assert.Equal<ContributorsTeamViewModel>(testViewModel, actual, new ContributorTeamViewModelComparer());
        }
    }
}
