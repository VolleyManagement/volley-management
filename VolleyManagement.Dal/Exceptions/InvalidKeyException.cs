namespace VolleyManagement.Dal.MsSql.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occurs during the searching entity id
    /// in database
    /// </summary>
    public class InvalidKeyFieldException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidIdException
        /// class.
        /// </summary>
        public InvalidKeyFieldException() :
            base("Specified value is not exist in database")
        {
        }
    }
}
