namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Class for generating test list of divisions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DivisionTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private List<Division> _divisions = new List<Division>();

        /// <summary>
        /// Return test collection of Divisions
        /// </summary>
        /// <returns>Builder object with collection of Divisions</returns>
        public DivisionTestFixture TestDivisions()
        {
            _divisions.Add(new Division()
            {
                Id = 1,
                Name = "Division 1",
                TournamentId = 1,
                Groups =
                {
                    new Group()
                    {
                        Id = 1,
                        Name = "Group1",
                        DivisionId = 1
                    },
                }
            });
            _divisions.Add(new Division()
            {
                Id = 2,
                Name = "Division 2",
                TournamentId = 1,
                Groups =
                {
                    new Group()
                    {
                        Id = 2,
                        Name = "Group 1",
                        DivisionId = 2
                    },
                }
            });
            return this;
        }

        /// <summary>
        /// Return test collection of Divisions with 1 division
        /// </summary>
        /// <returns>Builder object with collection of Divisions</returns>
        public DivisionTestFixture TestDivision()
        {
            _divisions.Add(new Division()
            {
                Id = 1,
                Name = "Division 1",
                TournamentId = 1,
                Groups =
                {
                    new Group()
                    {
                        Id = 1,
                        Name = "Group1",
                        DivisionId = 1
                    },
                }
            });
            return this;
        }

        /// <summary>
        /// Add Division to collection.
        /// </summary>
        /// <param name="newDivision">Division to add.</param>
        /// <returns>Builder object with collection of Divisions.</returns>
        public DivisionTestFixture AddDivision(Division newDivision)
        {
            _divisions.Add(newDivision);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Division collection</returns>
        public List<Division> Build()
        {
            return _divisions;
        }
    }
}
