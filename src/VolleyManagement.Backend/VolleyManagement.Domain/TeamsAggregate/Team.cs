using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VolleyManagement.Domain.Properties;
using static VolleyManagement.Domain.TeamsAggregate.TeamValidation;
using static VolleyManagement.Domain.Properties.Resources;

namespace VolleyManagement.Domain.TeamsAggregate
{
    /// <summary>
    /// Team domain class.
    /// </summary>
    public class Team
    {
        private string _name;
        private string _coach;
        private string _achievements;
        private PlayerId _captainId;
        private ICollection<PlayerId> roster;


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
            get => _name;

            set
            {
                if (ValidateTeamName(value))
                {
                    throw new ArgumentException(ValidationTeamName,
                        nameof(value));
                }

                _name = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Coach.
        /// </summary>
        /// <value>Coach of the team</value>
        public string Coach
        {
            get => _coach;

            set
            {
                if (ValidateCoachName(value))
                {
                    throw new ArgumentException(ValidationCoachName,
                        nameof(value));
                }

                _coach = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Achievements.
        /// </summary>
        /// <value>Achievements of the team</value>
        public string Achievements
        {
            get => _achievements;

            set
            {
                if (ValidateAchievements(value))
                {
                    throw new ArgumentException(ValidationTeamAchievements,
                        nameof(value));
                }

                _achievements = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Captain.
        /// </summary>
        /// <value>Captain of the team</value>
        public PlayerId CaptainId
        {
            get => _captainId;

            set
            {
                if (ValidateCaptainId(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerFirstName,
                        nameof(value));
                }

                _captainId = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating where team players.
        /// </summary>
        /// <value>Players of the team</value>
        public ICollection<PlayerId> Roster { get; set; }
    }
}
