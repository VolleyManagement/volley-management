namespace VolleyManagement.Contracts.Exceptions
{
    using System;
    using System.Collections;

    /// <summary>
    /// Represents errors that occurs during the searching entity in database
    /// </summary>
    public class MissingEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MissingEntityException
        /// class.
        /// </summary>
        public MissingEntityException() :
            base("Specified entity missing in database")
        {
        }

        /// <summary>
        /// Initializes a new instance of the MissingEntityException class.
        /// </summary>
        /// <param name="exception">Exception uses to initialize data MissingEntityException</param>
        public MissingEntityException(Exception exception) :
            base("Specified entity missing in database")
        {
            foreach(DictionaryEntry dataItem in exception.Data)
            {
                this.Data[dataItem.Key] = dataItem.Value;
            }
        }
    }
}
