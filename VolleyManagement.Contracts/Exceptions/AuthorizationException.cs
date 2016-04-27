namespace VolleyManagement.Contracts.Exceptions
{
    using System;

    /// <summary>
    /// Contains information about thrown authorization exception
    /// </summary>
    public class AuthorizationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException"/> class
        /// </summary>
        public AuthorizationException()
            : base("Requested operation is not allowed")
        {
        }
    }
}
