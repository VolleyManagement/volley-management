namespace VolleyManagement.Contracts.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents errors that occurs during the validation of the
    /// player.
    /// </summary>
    [Serializable]
    public class PlayerValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PlayerValidationException
        /// class.
        /// </summary>
        public PlayerValidationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PlayerValidationException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PlayerValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PlayerValidationException
        /// class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">Inner exception that is the cause of this
        /// exception</param>
        public PlayerValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PlayerValidationException
        /// class with a serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized data about the exception being thrown.</param>
        /// <param name="context">StreamingContext that contains the information about the source or destination.</param>
        public PlayerValidationException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
