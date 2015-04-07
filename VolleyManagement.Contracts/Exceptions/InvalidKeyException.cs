namespace VolleyManagement.Contracts.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occurs during the searching entity id
    /// in database
    /// </summary>
    public class InvalidKeyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class.
        /// </summary>
        public InvalidKeyException() :
            base("Specified key value does not exist in database")
        {
        }
    }
}
