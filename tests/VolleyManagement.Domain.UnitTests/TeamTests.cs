
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using Xunit;

namespace VolleyManagement.Domain.UnitTests
{
    public class TeamTests
    {
        private const int TEST_TEAM_ID = 1;
        private const string TEST_TEAM_NAME = "Team1";
        private const string TEST_COACH_NAME = "NameCoach";
        private const string TEST_TEAM_ACHIEVEMENTS = "";
        private const int TEST_TEAM_CAPTAIN_ID = 1;

        [Fact]
        public void Team_CreateValidInstance_TeamCreated()
        {
            //Arrange
            Team team = null;

            //Act
            team = CreateTeam();

            //Assert
            AssertCorrectTeamCreated(team);
        }

        #region Name

        [Fact]
        public void Team_EmptyName_ExceptionIsThrown()
        {
            // Arrange
            var team = CreateTeam();

            // Act
            Action act = () => { team.Name = string.Empty; };

            // Assert
            act.Should().Throw<ArgumentException>("Empty Name is not allowed.");
        }

        [Fact]
        public void Team_MaxNameLength_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Name = GenerateTeamStrings(Constants.Team.MAX_NAME_LENGTH + 1); };

            //Assert
            act.Should().Throw<ArgumentException>("Name length should not be longer than maximum allowed value.");
        }

        #endregion

        #region Coach

        [Fact]
        public void Team_MaxCoachName_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Coach = GenerateTeamStrings(Constants.Team.MAX_COACH_NAME_LENGTH + 1); };

            //Assert
            act.Should().Throw<ArgumentException>("Coach Name length should not be longer than maximum allowed value.");
        }

        [Fact]
        public void Team_CoachNameContainsNumber_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Coach = "Coah1"; };

            //Assert
            act.Should().Throw<ArgumentException>("Coach Name can`t contain numbers.");
        }

        [Fact]
        public void Team_CoachNameContainsSymbol_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Coach = "Coah#"; };

            //Assert
            act.Should().Throw<ArgumentException>("Coach Name can`t contain symbols.");
        }

        #endregion

        #region Achievements

        [Fact]
        public void Team_MaxAchievementsLength_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Achievements = GenerateTeamStrings(Constants.Team.MAX_ACHIEVEMENTS_LENGTH + 1); };

            //Assert
            act.Should().Throw<ArgumentException>("Achievements length should not be longer than maximum allowed value.");
        }

        [Fact]
        public void Team_EmptyAchievements_ExceptionIsNotThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.Achievements = string.Empty; };

            //Assert
            act.Should().NotThrow<ArgumentException>("Achievements length can be empty");
        }

        #endregion

        #region CaptainId

        [Fact]
        public void Team_EmptyCaptainId_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.SetCaptain(null); };

            //Assert
            act.Should().Throw<ArgumentException>("Set null  Captain not allowed");
        }

        [Fact]
        public void Team_MinCaptainId_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.SetCaptain(new PlayerId(Constants.Team.MIN_ID - 1)); };

            //Assert
            act.Should().Throw<ArgumentException>("CaptainId can`t be less then minimum alloved value");
        }

        #endregion

        #region AddPlayers

        [Fact]
        public void Team_RosterIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.AddPlayers(null); };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t be null.");
        }

        [Fact]
        public void Team_MinPlayerID_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () =>
            {
                team.AddPlayers(new List<PlayerId>() {
                    new PlayerId(Constants.Team.MIN_ID-1)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team's roster is wrong");

        }

        [Fact]
        public void Team_RosterContainsAlreadyTheSamePlayerId_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            //Act
            Action act = () =>
            {
                team.AddPlayers(new List<PlayerId>() {
                    new PlayerId(2),
                    new PlayerId(2)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t has players with the same Id.");
        }

        [Fact]
        public void Team_RosterHasAlreadyThisPlayer_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            team.AddPlayers(new List<PlayerId>() {
                new PlayerId(3),
                new PlayerId(4)
            });

            //Act
            Action act = () =>
            {
                team.AddPlayers(new List<PlayerId>() {
                    new PlayerId(4)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t be null.");
        }

        [Fact]
        public void Team_PlayerToAddIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            var listRemovePlayers = new List<PlayerId>() {
                null
            };

            //Act
            Action act = () =>
            {
                team.AddPlayers(listRemovePlayers);
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t be null.");
        }

        #endregion

        #region RemovePlayers

        [Fact]
        public void Team_RemoveRosterIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.RemovePlayers(null); };

            //Assert
            act.Should().Throw<ArgumentException>("Parametr in RemovePlayers can`t be null.");
        }

        [Fact]
        public void Team_RemoveCaptain_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    new PlayerId(team.Captain.Id)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team RomovePlayers can`t remove captain");

        }

        [Fact]
        public void Team_RemoveRosterContainsDuplicate_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    new PlayerId(2),
                    new PlayerId(2)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t delete duplicate players.");
        }

        [Fact]
        public void Team_RemovePlayerNotContainsInRoster_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            team.AddPlayers(new List<PlayerId>() {
                new PlayerId(4),
                new PlayerId(5)
                
            });
            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    new PlayerId(4),
                    new PlayerId(6)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t delete players that doesn`t have.");
        }

        [Fact]
        public void Team_PlayerToRemoveIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            var listRemovePlayers = new List<PlayerId>() {
                null
            };

            //Act
            Action act = () =>
            {
                team.RemovePlayers(listRemovePlayers);
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t delete list that contains null.");
        }

        [Fact]
        public void Team_PlayerIdToRemoveIsNotValid_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    new PlayerId(Constants.Team.MIN_ID-1)
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t delete player with not valid Id.");
        }

        #endregion

        #region Private methods

        private void AssertCorrectTeamCreated(Team actual)
        {
            actual.Id.Should().Be(TEST_TEAM_ID, "Team's id wasn't set properly.");
            actual.Name.Should().Be(TEST_TEAM_NAME, "Team's name wasn't set properly.");
            actual.Coach.Should().Be(TEST_COACH_NAME, "Team's coach name wasn't set properly.");
            actual.Achievements.Should().Be(TEST_TEAM_ACHIEVEMENTS, "Team's achievements weren't set properly.");
            actual.Captain.Id.Should().Be(TEST_TEAM_CAPTAIN_ID, "Team's captain id wasn't set properly.");
            actual.Roster.Should().BeEquivalentTo(GetTeamRoster(), "Team's roster wasn't set properly");
        }

        private List<PlayerId> GetTeamRoster()
        {
            return new List<PlayerId>() { new PlayerId(TEST_TEAM_CAPTAIN_ID) };
        }

        private string GenerateTeamStrings(int length)
        {
            return string.Concat(System.Linq.Enumerable.Repeat("a", length));
        }

        private static Team CreateTeam()
        {
            var captainId = new PlayerId(1);
            return new Team(1, "Team1", "NameCoach", "", captainId, new List<PlayerId>());
        }

        #endregion
    }
}
