﻿namespace VolleyManagement.Data.Queries.Player
{
    using Contracts;

    /// <summary>
    /// Get By First and Last names type queries parameters
    /// </summary>
    public class FindByFullNameCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets FirstName of the Player
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets SecondName of the Player
        /// </summary>
        public string LastName { get; set; }
    }
}
