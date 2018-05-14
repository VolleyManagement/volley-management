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
        private readonly ICollection<PlayerId> _roster = new List<PlayerId>();

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
                throw new EntityInvariantViolationException();
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
                    throw new EntityInvariantViolationException();
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
                    throw new EntityInvariantViolationException();
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
                    throw new EntityInvariantViolationException();
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
        public IEnumerable<PlayerId> Roster => _roster;

        public void SetCaptain(PlayerId captain)
        {
            if (ValidateCaptain(captain))
            {
                throw new EntityInvariantViolationException();
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
                throw new EntityInvariantViolationException();
            }

            if (RosterConstainsAny(players))
            {
                throw new EntityInvariantViolationException(AddingMemberedPlayerToTeam);
            }

            foreach (var player in players)
            {
                _roster.Add(player);
            }
        }

        public void RemovePlayers(IEnumerable<PlayerId> players)
        {
            if (ValidateTeamRoster(players))
            {
                throw new EntityInvariantViolationException();
            }

            if (!RosterContainsAll(players))
            {
                throw new EntityInvariantViolationException(RemovingUnmemberedPlayerFromTeam);
            }

            var playersIds = players.Select(x => x.Id);

            if (playersIds.Contains(Captain.Id))
            {
                throw new EntityInvariantViolationException(RemovingCaptain);
            }

            foreach (var playerId in playersIds)
            {
                _roster.Remove(_roster.First(x => x.Id == playerId));
            }
        }

        private bool RosterConstainsAny(IEnumerable<PlayerId> players) =>
            _roster.Select(x => x.Id)
                .Intersect(players.Select(y => y.Id))
                .Any();

        private bool RosterContainsAll(IEnumerable<PlayerId> players) =>
            players.Count() == _roster.Select(x => x.Id)
                .Intersect(players.Select(y => y.Id))
                .Count();
    }
}
