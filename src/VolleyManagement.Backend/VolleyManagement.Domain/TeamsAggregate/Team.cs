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
        private ICollection<PlayerId> _roster;

        /// <summary>
        /// Initializes a new instance of the Team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="coach"></param>
        /// <param name="achievements"></param>
        /// <param name="captain"></param>
        /// <param name="roster"></param>
        public Team(int id, string name, string coach, string achievements, PlayerId captain, IEnumerable<PlayerId> roster)
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
            AddPlayers(roster);
            SetCaptain(captain);
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
        public PlayerId Captain { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating where team players.
        /// </summary>
        /// <value>Players of the team</value>
        public IEnumerable<PlayerId> Roster
        {
            get => _roster;
        }

        public void SetCaptain(PlayerId captain)
        {
            if (ValidateCaptainId(captain))
            {
                throw new ArgumentException(ValidationTeamCaptain,
                    nameof(captain));
            }

            Captain = captain;

            if (!_roster.Select(x => x.Id).Contains(captain.Id))
            {
                _roster.Add(captain);
            }
        }

        public void AddPlayers(IEnumerable<PlayerId> players)
        {
            if (ValidateTeamRoster(players))
            {
                throw new ArgumentException(ValidationTeamRoster,
                    nameof(players));
            }

            var rosterIds = _roster.Select(x => x.Id);

            foreach (var player in players)
            {
                if (rosterIds.Contains(player.Id))
                {
                    throw new ArgumentException(AddingExistingPlayerToTeam);
                }

                _roster.Add(player);
            }
        }

        public void RemovePlayers(IEnumerable<PlayerId> players)
        {
            foreach (var player in players)
            {
                var toRemove = _roster.FirstOrDefault(x => x.Id == player.Id);

                if (toRemove != null &&
                    toRemove.Id == Captain.Id)
                {
                    throw new ArgumentException(RemovingCaptain);
                }

                _roster.Remove(toRemove);
            }
        }
    }
}
