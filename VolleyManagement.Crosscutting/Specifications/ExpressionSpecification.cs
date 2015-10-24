namespace VolleyManagement.Crosscutting.Specifications
{
    using System;

    /// <summary>
    /// Provides a way to use lambda as specification
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSpecification{T}"/> class.
        /// </summary>
        /// <param name="expression"> Original specification </param>
        public ExpressionSpecification(Func<T, bool> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            this._expression = expression;
        }

        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public override bool IsSatisfiedBy(T entity)
        {
            return _expression(entity);
        }
    }
}