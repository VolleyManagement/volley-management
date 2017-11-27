namespace VolleyManagement.Crosscutting.Specifications
{
    using Contracts.Specifications;

    /// <summary>
    /// The composite specification.
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public abstract bool IsSatisfiedBy(T entity);

        /// <summary>
        /// Combines two specifications using AND logical operator
        /// </summary>
        /// <param name="specification">Specification to add</param>
        /// <returns>Combined specification</returns>
        public CompositeSpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Combines two specifications using OR logical operator
        /// </summary>
        /// <param name="specification">Specification to add</param>
        /// <returns>Combined specification</returns>
        public CompositeSpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Reverses current specification
        /// </summary>
        /// <returns>Combined specification</returns>
        public CompositeSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}