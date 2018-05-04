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
}
}
