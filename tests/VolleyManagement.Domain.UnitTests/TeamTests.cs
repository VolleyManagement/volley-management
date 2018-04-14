using System;
using System.Collections.Generic;
using FluentAssertions;
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
            act.Should().Throw<ArgumentException>("Team should always have captain");
        }

        [Fact]
        public void Team_MinCaptainId_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.SetCaptain(GetInvalidPlayerId()); };

            //Assert
            act.Should().Throw<ArgumentException>("CaptainId can`t be less then minimum allowed value. Should be valid ID.");
        }

        #endregion

        #region Roster

        [Fact]
        public void Team_ListOfPlayersToAddIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.AddPlayers(null); };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t be null.");
        }

        [Fact]
        public void Team_PlayerIDLessThanMin_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () =>
            {
                team.AddPlayers(new List<PlayerId>() {
                    GetInvalidPlayerId()
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("It`s impossible add to team players that has id less than allowed");

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
                    GetPlayerIdTwo(),
                    GetPlayerIdTwo()
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t has players with the same Id.");
        }

        [Fact]
        public void Team_PlayerIsAlreadyPlaysInTeam_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            team.AddPlayers(new List<PlayerId>() {
                GetPlayerIdThree(),
                GetPlayerIdFour()
            });

            //Act
            Action act = () =>
            {
                team.AddPlayers(new List<PlayerId>() {
                    GetPlayerIdFour()
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("It`s impossible add player that already plays in team.");
        }

        [Fact]
        public void Team_PlayerToAddIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            var listAddPlayers = new List<PlayerId>() {
                null
            };

            //Act
            Action act = () =>
            {
                team.AddPlayers(listAddPlayers);
            };

            //Assert
            act.Should().Throw<ArgumentException>("It's impossible add rosters players that is null.");
        }

        [Fact]
        public void Team_RemoveListPlayersIsNull_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();

            //Act
            Action act = () => { team.RemovePlayers(null); };

            //Assert
            act.Should().Throw<ArgumentException>("It's impossible remove rosters players that is null.");
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
            act.Should().Throw<ArgumentException>("Team can`t be without captain, it`s impossible remove captain from team.");

        }

        [Fact]
        public void Team_RemoveDuplicatePlayers_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    GetPlayerIdTwo(),
                    GetPlayerIdTwo()
                });
            };

            //Assert
            act.Should().Throw<ArgumentException>("Team Roster can`t delete duplicate players.");
        }

        [Fact]
        public void Team_RemovePlayerWhichIsNotPartOfTheTeam_ExceptionIsThrown()
        {
            //Arrange
            var team = CreateTeam();
            team.AddPlayers(new List<PlayerId>() {
                GetPlayerIdFour(),
                GetPlayerIdTwo()
                
            });
            //Act
            Action act = () =>
            {
                team.RemovePlayers(new List<PlayerId>() {
                    GetPlayerIdFour(),
                    GetPlayerIdThree()
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
                    GetInvalidPlayerId()
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

        private PlayerId GetPlayerIdFour()
        {
            return new PlayerId(4);
        }

        private PlayerId GetPlayerIdThree()
        {
            return new PlayerId(3);
        }

        private PlayerId GetPlayerIdTwo()
        {
            return new PlayerId(2);
        }


        private PlayerId GetInvalidPlayerId()
        {
             return new PlayerId(Constants.Team.MIN_ID-1);
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
