namespace VolleyManagement.Domain.Teams
{
    using System;
using System.Collections.Generic;
using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Team domain class.
    /// </summary>
    public class Team
    {
        private string _name;
        private string _coach;
        private string _achievements;
        private int _captainId;

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
        public int CaptainId
        {
            get
            {
                return _captainId;
            }

            set
            {
                _captainId = value;
            }
        }
    }
}
