namespace VolleyManagement.Data.Exceptions
{
    using System;

    /// <summary>
    /// The invalid entity exception.
    /// </summary>
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException()
            : base("Entity can not be stored in the database")
        {
        }
    }
}