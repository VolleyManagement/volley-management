namespace VolleyManagement.Data.Exceptions
{
    using System;

    /// <summary>
    /// Notifies that update operation has failed
    /// because corresponding record in DB has changed from last known state
    /// </summary>
    [Serializable]
    public class ConcurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException()
            : base("Optimistic concurrency check failed. Entity has been changed.")
        {
        }
    }
}