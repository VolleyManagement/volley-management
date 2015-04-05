namespace VolleyManagement.Domain.Specifications
{
    /// <summary>
    /// The Specification interface.
    /// </summary>
    /// <typeparam name="T"> Type for which specification belongs </typeparam>
    public interface ISpecification<in T>
    {
        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> The <see cref="bool"/>. </returns>
        bool IsSatisfiedBy(T entity);
    }
}
