namespace VolleyManagement.Domain.Teams
{
    using System;
using System.Collections.Generic;
using VolleyManagement.Domain.Players;
using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Team domain class.
    /// </summary>
    public class Team
    {
        private string _name;
        private string _coach;
        private string _achievements;
        private Player _captain;
        private IEnumerable<Player> _roster;

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of team.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of the team</value>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Coach.
        /// </summary>
        /// <value>Coach of the team</value>
        public string Coach
        {
            get
            {
                return _coach;
            }

            set
            {
                _coach = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Achievements.
        /// </summary>
        /// <value>Achievements of the team</value>
        public string Achievements
        {
            get
            {
                return _achievements;
            }

            set
            {
                _achievements = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Captain.
        /// </summary>
        /// <value>Captain of the team</value>
        public Player Captain
        {
            get
            {
                return _captain;
            }

            set
            {
                _captain = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Roster.
        /// </summary>
        /// <value>Roster of the team</value>
        public IEnumerable<Player> Roster
        {
            get
            {
                return _roster;
            }

            set
            {
                _roster = value;
            }
        }
    }
}
