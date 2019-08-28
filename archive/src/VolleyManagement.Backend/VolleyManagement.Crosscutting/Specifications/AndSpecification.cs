namespace VolleyManagement.Crosscutting.Specifications
{
    using System;
    using Contracts.Specifications;

    /// <summary>
    /// Provides a way to chain two Specifications using AND operator
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _left;

        private readonly ISpecification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
        /// </summary>
        /// <param name="left"> Left operand </param>
        /// <param name="right"> Right operand </param>
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            _left = left;
            _right = right;
        }

        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public override bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity)
                && _right.IsSatisfiedBy(entity);
        }
    }
}