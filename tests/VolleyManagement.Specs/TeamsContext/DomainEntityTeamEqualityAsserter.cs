namespace VolleyManagement.Specs.TeamsContext
{
    using FluentAssertions;
    using System.Collections.Generic;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.TeamsAggregate;

    using static System.Linq.Enumerable;

    /// <summary>
    /// Checks if domain team equals team entity.
    /// </summary>
    static class DomainEntityTeamEqualityAsserter
    {
        public static void AssertSimpleDataIsEqual(TeamEntity entity, Team domain)
        {
            entity.Id.Should().Be(domain.Id);
            entity.Name.Should().Be(domain.Name);
            entity.Coach.Should().Be(domain.Coach);
            entity.Achievements.Should().Be(domain.Achievements);
            entity.CaptainId.Should().Be(domain.Captain.Id);
        }

        public static void AssertRosterIsEqual(ICollection<PlayerEntity> entity, ICollection<PlayerId> domain)
        {
            entity.Count.Should().Be(domain.Count, "Roster's count must be equal.");

            var entityRosterIds = entity.Select(e => e.Id);
            var domainRosterIds = domain.Select(d => d.Id);
            entityRosterIds.Should().BeEquivalentTo(domainRosterIds);
        }
    }
}
