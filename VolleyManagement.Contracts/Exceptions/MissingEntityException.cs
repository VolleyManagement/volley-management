namespace VolleyManagement.Contracts.Exceptions
{
    using System;

    using VolleyManagement.Domain;

    /// <summary>
    /// Represents errors that occurs during the searching entity in database
    /// </summary>
    public sealed class MissingEntityException : Exception
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
        /// <param name="message">Exception uses to initialize data MissingEntityException</param>
        public MissingEntityException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingEntityException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (innerException.Data.Contains(Constants.ExceptionManagement.ENTITY_ID_KEY))
            {
                AddEntityIdToData(innerException.Data[Constants.ExceptionManagement.ENTITY_ID_KEY]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingEntityException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="entityId">Id related to error occur</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingEntityException(string message, int? entityId, Exception innerException)
            : base(message, innerException)
        {
            AddEntityIdToData(entityId);
        }

        private void AddEntityIdToData(object entityId)
        {
            this.Data[Constants.ExceptionManagement.ENTITY_ID_KEY] = entityId;
        }

        private void AddEntityIdToData(int entityId)
        {
            this.Data[Constants.ExceptionManagement.ENTITY_ID_KEY] = entityId;
        }
    }
}
