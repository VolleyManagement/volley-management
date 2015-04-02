namespace VolleyManagement.Dal.MsSql.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occurs during the searching entity id
    /// in database
    /// </summary>
    public class InvalidIdException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the InvalidIdException
        /// class.
        /// </summary>
        public InvalidIdException() :
            base("Specified id is not exist")
        {
        }
    }
}
