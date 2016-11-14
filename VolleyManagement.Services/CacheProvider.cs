namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Web;
    using VolleyManagement.Contracts;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Provides the way to work with application.
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        /// <summary>
        /// Gets list of user indexes from application.
        /// </summary>
        /// <param name="value">Key for application.</param>
        /// <returns>List of all users id.</returns>
        public List<int> this[string value]
        {
            get
            {
                if (HttpContext.Current.Application[value] == null)
                {
                    HttpContext.Current.Application[value] = new List<int>();
                }

                return (List<int>)HttpContext.Current.Application[value];
            }
        }
    }
}