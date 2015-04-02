namespace VolleyManagement.Dal.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occurs during the searching entity id
    /// in database
    /// </summary>
    public class InvalidKeyValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class.
        /// </summary>
        public InvalidKeyValueException() :
            base("Specified key value is not exist in database")
        {
        }
    }
}
