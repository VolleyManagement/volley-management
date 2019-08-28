namespace VolleyManagement.Services
{
    using System.Web;
    using Contracts;

    /// <summary>
    /// Provides the way to work with application.
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        /// <summary>
        /// Gets or sets list of user indexes from application.
        /// </summary>
        /// <param name="key">Key for application.</param>
        /// <returns>List of all users id.</returns>
        public object this[string key]
        {
            get
            {
                return HttpContext.Current.Application[key];
            }

            set
            {
                HttpContext.Current.Application[key] = value;
            }
        }
    }
}