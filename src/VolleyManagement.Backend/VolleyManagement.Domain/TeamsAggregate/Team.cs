using System;
using System.Collections.Generic;
using System.Linq;
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
        private ICollection<PlayerId> _roster;

        /// <summary>
        /// Initializes a new instance of the Team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="coach"></param>
        /// <param name="achievements"></param>
        /// <param name="captainId"></param>
        /// <param name="roster"></param>
        public Team(int id, string name, string coach, string achievements, PlayerId captainId, IEnumerable<PlayerId> roster)
        {
            if (ValidateTeamId(id))
            {
                throw new ArgumentException(ValidationTeamId,
                nameof(id));
            }

            Id = id;
            Name = name;
            Coach = coach;
            Achievements = achievements;
            CaptainId = new PlayerId(captainId.Id);
            Roster = roster.Select(x => new PlayerId(x.Id)).ToList();
            if (!Roster.Contains(CaptainId))
            {
                Roster.Add(CaptainId);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of team.</value>
        public int Id { get; }

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
                    throw new ArgumentException(ValidationTeamCaptain,
                        nameof(value));
                }

                _captainId = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating where team players.
        /// </summary>
        /// <value>Players of the team</value>
        public ICollection<PlayerId> Roster
        {
            get => _roster;

            set
            {
                if (ValidateTeamRoster(value))
                {
                    throw new ArgumentException(ValidationTeamRoster,
                        nameof(value));
                }

                _roster = value;

            }
        }
    }
}
