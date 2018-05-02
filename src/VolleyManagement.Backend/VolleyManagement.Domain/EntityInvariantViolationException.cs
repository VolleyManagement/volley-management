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

        ///// <summary>
        ///// Initializes a new instance of the <see cref="EntityInvariantViolationException"/> class.
        ///// </summary>
        ///// <param name="message">The message.</param>
        ///// <param name="entityId">Id related to error occur</param>
        //public EntityInvariantViolationException(int? entityId)
        //    : base($"Team with id {entityId} is not valid ")
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="EntityInvariantViolationException"/> class.
        ///// </summary>
        ///// <param name="message">The message.</param>
        ///// <param name="entityName">Id related to error occur</param>
        //public EntityInvariantViolationException(string entityName)
        //    : base($"Team with name {entityName} is not valid ")
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="EntityInvariantViolationException"/> class.
        ///// </summary>
        ///// <param name="message">The message.</param>
        ///// <param name="entityName">Id related to error occur</param>
        //public EntityInvariantViolationException(string entityCoachName, int a)
        //    : base($"Team with name {entityCoachName} is not valid ")
        //{
        //}

}
}
