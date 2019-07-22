﻿namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represent entity of a group
    /// </summary>
    public class GroupEntity
    {
        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets division name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets group's division id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets group's division
        /// </summary>
        public virtual DivisionEntity Division { get; set; }

        /// <summary>
        /// Gets or sets collection of tournaments teams
        /// </summary>
        public virtual ICollection<TeamEntity> Teams { get; set; }
    }
}
