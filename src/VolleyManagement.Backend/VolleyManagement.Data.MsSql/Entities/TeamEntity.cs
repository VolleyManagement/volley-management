﻿namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// DAL team model
    /// </summary>
    public class TeamEntity
    {
        /// <summary>
        /// Gets or sets id of team
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets coach of team
        /// </summary>
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets achievements of team
        /// </summary>
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets Captain of the team
        /// </summary>
        public int CaptainId { get; set; }

        /// <summary>
        /// Gets or sets Captain of the team
        /// </summary>
        public virtual PlayerEntity Captain { get; set; }

        /// <summary>
        /// Gets or sets players of the team
        /// </summary>
        public virtual ICollection<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();

        /// <summary>
        /// Gets or sets home game results of the team.
        /// </summary>
        public virtual ICollection<GameResultEntity> HomeGameResults { get; set; }

        /// <summary>
        /// Gets or sets away game results of the team.
        /// </summary>
        public virtual ICollection<GameResultEntity> AwayGameResults { get; set; }

        /// <summary>
        /// Gets or sets groups in which team takes part.
        /// </summary>
        public virtual ICollection<GroupEntity> Groups { get; set; }
    }
}