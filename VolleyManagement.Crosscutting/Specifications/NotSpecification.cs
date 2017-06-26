namespace VolleyManagement.Crosscutting.Specifications
{
    using System;

    using Contracts.Specifications;

    /// <summary>
    /// Provides a way to chain two Specifications using NOT operator
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _original;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
        /// </summary>
        /// <param name="original"> Original specification </param>
        public NotSpecification(ISpecification<T> original)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }

            _original = original;
        }

        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public override bool IsSatisfiedBy(T entity)
        {
            return !_original.IsSatisfiedBy(entity);
        }
    }
}