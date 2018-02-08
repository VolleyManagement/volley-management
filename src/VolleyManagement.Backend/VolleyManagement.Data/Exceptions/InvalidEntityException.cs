namespace VolleyManagement.Data.Exceptions
{
    using System;

    /// <summary>
    /// Notifies that DB save attempt was un-successful due to invalid values of provided entity
    /// </summary>
    [Serializable]
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