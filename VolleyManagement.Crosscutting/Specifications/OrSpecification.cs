namespace VolleyManagement.Crosscutting.Specifications
{
    using System;

    using VolleyManagement.Crosscutting.Contracts.Specifications;

    /// <summary>
    /// Provides a way to chain two Specifications using OR operator
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _left;

        private readonly ISpecification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
        /// </summary>
        /// <param name="left"> Left operand </param>
        /// <param name="right"> Right operand </param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            this._left = left;
            this._right = right;
        }

        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public override bool IsSatisfiedBy(T entity)
        {
            return this._left.IsSatisfiedBy(entity)
                   || this._right.IsSatisfiedBy(entity);
        }
    }
}