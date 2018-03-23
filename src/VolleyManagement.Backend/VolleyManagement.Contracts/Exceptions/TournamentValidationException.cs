﻿namespace VolleyManagement.Contracts.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents errors that occurs during the validation of the
    /// tournament.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Usage",
        "CA2240:ImplementISerializableCorrectly",
        Justification = "It asks to override GetObjectData - we do not use it serialization scenarios and we're going to aovid using exceptions")]
    [Serializable]
    public class TournamentValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TournamentValidationException
        /// class.
        /// </summary>
        public TournamentValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TournamentValidationException
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TournamentValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TournamentValidationException
        /// class with a specified error message and a name of not valid parameter.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="validationKey">The key in validation dictionary</param>
        /// <param name="paramName">The not valid parameter name.</param>
        public TournamentValidationException(string message, string validationKey, string paramName)
            : base(message)
        {
            ParamName = paramName;
            ValidationKey = validationKey;
        }

        /// <summary>
        /// Initializes a new instance of the TournamentValidationException
        /// class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">Inner exception that is the cause of this
        /// exception</param>
        public TournamentValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TournamentValidationException
        /// class with a serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized data about the exception being thrown.</param>
        /// <param name="context">StreamingContext that contains the information about the source or destination.</param>
        public TournamentValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets name of a not valid parameter
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// Gets or sets the key to message in a dictionary
        /// </summary>
        public string ValidationKey { get; set; }
    }
}
