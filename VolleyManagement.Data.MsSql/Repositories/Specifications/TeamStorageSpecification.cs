namespace VolleyManagement.Data.MsSql.Repositories.Specifications
{
    using VolleyManagement.Crosscutting.Specifications;
    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// Specifies Team storage requirements
    /// </summary>
    public class TeamStorageSpecification : ISpecification<TeamEntity>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public bool IsSatisfiedBy(TeamEntity entity)
        {
            var name = new ExpressionSpecification<TeamEntity>(t =>
                                !string.IsNullOrEmpty(t.Name)
                                && t.Name.Length < ValidationConstants.Team.MAX_NAME_LENGTH);

            var coach = new ExpressionSpecification<TeamEntity>(t =>
                                string.IsNullOrEmpty(t.Coach)
                                || t.Coach.Length < ValidationConstants.Team.MAX_COACH_NAME_LENGTH);

            var achievements = new ExpressionSpecification<TeamEntity>(t =>
                                string.IsNullOrEmpty(t.Achievements)
                                || t.Achievements.Length < ValidationConstants.Team.MAX_ACHIEVEMENTS_LENGTH);

            return name.And(coach).And(achievements)
                .IsSatisfiedBy(entity);
        }
    }
}