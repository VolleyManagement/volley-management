namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;
    using VolleyManagement.UnitTests.Services.TeamService;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GroupTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private List<Group> _groups = new List<Group>();

        /// <summary>
        /// Return test collection of groups
        /// </summary>
        /// <returns>Builder object with collection of groups</returns>
        public GroupTestFixture TestGroups()
        {
            _groups.Add(new Group()
            {
                DivisionId = 1,
                Id = 1,
                Name = "Group 1"
            });
            _groups.Add(new Group()
            {
                DivisionId = 1,
                Id = 2,
                Name = "Group 2"
            });
            return this;
        }

        /// <summary>
        /// Add group to collection.
        /// </summary>
        /// <param name="newGroup">Group to add.</param>
        /// <returns>Builder object with collection of groups.</returns>
        public GroupTestFixture AddGroup(Group newGroup)
        {
            _groups.Add(newGroup);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team collection</returns>
        public List<Group> Build()
        {
            return _groups;
        }
    }
}