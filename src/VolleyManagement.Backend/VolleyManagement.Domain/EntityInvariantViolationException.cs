namespace VolleyManagement.Domain.TeamsAggregate
{
    using System;

    /// <summary>
    /// Contains information about thrown authorization exception
    /// </summary>
    [Serializable]
    public class EntityInvariantViolationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInvariantViolationException"/> class
        /// </summary>
        public EntityInvariantViolationException()
            : base("Entity Invariant Violation")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInvariantViolationException"/> class.
        /// </summary>
        /// <param name="message">Id related to error occur</param>
        public EntityInvariantViolationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EntityInvariantViolationException
        /// class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">Inner exception that is the cause of this
        /// exception</param>
        public EntityInvariantViolationException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
