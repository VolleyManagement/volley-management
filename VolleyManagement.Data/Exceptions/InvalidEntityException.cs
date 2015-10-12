namespace VolleyManagement.Data.Exceptions
{
    using System;

    /// <summary>
    /// The invalid entity exception.
    /// </summary>
    public class InvalidEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityException"/> class.
        /// </summary>
        public InvalidEntityException()
            : base("Entity can not be stored in the database")
        {
        }
    }
}