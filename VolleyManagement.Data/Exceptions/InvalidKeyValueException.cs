namespace VolleyManagement.Data.Exceptions
{
    using System;

    using VolleyManagement.Data.Contracts;

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
            base("Specified key value does not exist in database")
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class with message and inner exception
        /// </summary>
        /// <param name="message">Message text</param>
        public InvalidKeyValueException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class with message and inner exception
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="innerException">Original exception</param>
        public InvalidKeyValueException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class with message and inner exception
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="entityId">Id related to error occur</param>
        public InvalidKeyValueException(string message, int entityId) :
            base(message)
        {
            this.AddEntityIdToData(entityId);
        }

        /// <summary>
        /// Initializes a new instance of the InvalidKeyValueException
        /// class with message and inner exception
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="entityId">Id related to error occur</param>
        /// <param name="innerException">Original exception</param>
        public InvalidKeyValueException(string message, int entityId, Exception innerException) :
            base(message, innerException)
        {
            this.AddEntityIdToData(entityId);
        }

        private void AddEntityIdToData(int entityId)
        {
            this.Data[Constants.ENTITY_ID_KEY] = entityId;
        }
    }
}
