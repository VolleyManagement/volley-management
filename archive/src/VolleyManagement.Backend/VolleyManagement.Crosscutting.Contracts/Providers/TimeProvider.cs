namespace VolleyManagement.Crosscutting.Contracts.Providers
{
    using System;

    /// <summary>
    /// Represents time provider class
    /// </summary>
    public abstract class TimeProvider
    {
        private static TimeProvider _current = new DefaultTimeProvider();

        /// <summary>
        /// Gets or sets current time provider
        /// </summary>
        public static TimeProvider Current
        {
            get
            {
                return _current;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _current = value;
            }
        }

        /// <summary>
        /// Gets current UTC time
        /// </summary>
        public abstract DateTime UtcNow { get; }

        /// <summary>
        /// Gets current time zone time.
        /// </summary>
        public DateTime DateTimeNow
        {
            get
            {
                return UtcNow.ToLocalTime();
            }
        }

        /// <summary>
        /// Sets default time provider
        /// </summary>
        public static void ResetToDefault()
        {
            _current =
              new DefaultTimeProvider();
        }
    }
}
