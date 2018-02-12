namespace VolleyManagement.Crosscutting.Contracts.Providers
{
    using System;

    /// <summary>
    /// Represents default time provider object
    /// </summary>
    internal sealed class DefaultTimeProvider : TimeProvider
    {
        /// <summary>
        /// Gets current UTC date
        /// </summary>
        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}