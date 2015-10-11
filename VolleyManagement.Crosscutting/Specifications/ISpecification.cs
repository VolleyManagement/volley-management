namespace VolleyManagement.Crosscutting.Specifications
{
    /// <summary>
    /// The Specification interface.
    /// </summary>
    /// <typeparam name="T"> Type for which specification belongs </typeparam>
    public interface ISpecification<in T>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        bool IsSatisfiedBy(T entity);
    }
}
