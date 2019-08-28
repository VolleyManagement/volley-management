namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using Domain.ContributorsAggregate;
    using Xunit;
    using Services.ContributorService;

    /// <summary>
    /// View model contributor team class test
    /// </summary>
    public class ContributorTeamViewModelToDomain
    {
        /// <summary>
        /// ToDomain() method test.
        /// Does correct contributor team view model mapped to domain model.
        /// </summary>
        [Fact]
        public void ToDomain_ContributorTeamViewModel_MappedToDomain()
        {
            // Arrange
            var testViewModel = new ContributorTeamMvcViewModelBuilder()
                .WithId(1)
                .WithName("First")
                .WithCourseDirection("Course")
                .WithContributors(new List<Contributor>
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
                })
                .Build();

            var testDomainModel = new ContributorTeamBuilder()
               .WithId(1)
               .WithName("First")
               .WithcourseDirection("Course")
               .Withcontributors(new List<Contributor>
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
               })
               .Build();

            // Act
            var actual = testViewModel.ToDomain();

            // Assert
            Assert.Equal<ContributorTeam>(testDomainModel, actual, new ContributorTeamComparer());
        }
    }
}
