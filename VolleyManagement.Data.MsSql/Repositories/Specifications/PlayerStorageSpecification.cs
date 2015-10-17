namespace VolleyManagement.Data.MsSql.Repositories.Specifications
{
    using VolleyManagement.Crosscutting.Contracts.Specifications;
    using VolleyManagement.Crosscutting.Specifications;
    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// Provides Specification for DB storage
    /// </summary>
    public class PlayerStorageSpecification : ISpecification<PlayerEntity>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public bool IsSatisfiedBy(PlayerEntity entity)
        {
            var fName = new ExpressionSpecification<PlayerEntity>(p =>
                                !string.IsNullOrEmpty(p.FirstName)
                                && p.FirstName.Length <= ValidationConstants.Player.MAX_FIRST_NAME_LENGTH);
            var lName = new ExpressionSpecification<PlayerEntity>(p =>
                                !string.IsNullOrEmpty(p.LastName)
                                && p.LastName.Length <= ValidationConstants.Player.MAX_LAST_NAME_LENGTH);
            return fName.And(lName).IsSatisfiedBy(entity);
        }
    }
}