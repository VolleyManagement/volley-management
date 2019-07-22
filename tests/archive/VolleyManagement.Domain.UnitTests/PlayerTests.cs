using System;
using FluentAssertions;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using Xunit;

namespace VolleyManagement.Domain.UnitTests
{
    public class PlayerTests
    {
        private const int TEST_PLAYER_ID = 1;
        private const string TEST_PLAYER_FIRST_NAME = "Ivan";
        private const string TEST_PLAYER_LAST_NAME = "Ivanov";
        private const short TEST_PLAYER_BIRTH_YEAR = 1988;
        private const short TEST_PLAYER_HEIGHT = 100;
        private const short TEST_PLAYER_WEIGHT = 80;

        [Fact]
        public void Player_CreateValidInstance_PlayerCreated()
        {
            // Arrange
            Player player;

            // Act
            player = CreatePlayer();

            // Assert
            AssertCorrectPlayerCreated(player);
        }

        [Fact]
        public void Player_BirthDateIsNull_ExceptionIsNotThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.BirthYear = null; };

            // Assert
            act.Should().NotThrow<EntityInvariantViolationException>("Player's birth date can be null.");
        }

        [Fact]
        public void Player_HeightIsNull_ExceptionIsNotThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = null; };

            // Assert
            act.Should().NotThrow<EntityInvariantViolationException>("Player's height can be null.");
        }

        [Fact]
        public void Player_WeightIsNull_ExceptionIsNotThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Weight = null; };

            // Assert
            act.Should().NotThrow<EntityInvariantViolationException>("Player's weight can be null.");
        }

        #region FirstName

        [Fact]
        public void Player_EmptyFirstName_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = string.Empty; };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("Empty first name is not allowed.");
        }

        [Fact]
        public void Player_FirstNameContainsNumber_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = "Player1"; };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("First name cannot contain number.");
        }

        [Fact]
        public void Player_FirstNameLengthGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerName(Constants.Player.MAX_FIRST_NAME_LENGTH + 1); };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("First name length should not be longer than maximum allowed value.");
        }

        #endregion

        #region LastName

        [Fact]
        public void Player_EmptyLastName_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.LastName = string.Empty; };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("Empty last name is not allowed.");
        }

        [Fact]
        public void Player_LastNameContainsNumber_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.LastName = "Player1"; };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("Last name cannot contain number.");
        }

        [Fact]
        public void Player_LastNameLengthGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerName(Constants.Player.MAX_LAST_NAME_LENGTH + 1); };

            // Assert
            act.Should().Throw<EntityInvariantViolationException>("Last name length should not be longer than maximum allowed value.");
        }

        #endregion

        #region BirthYear

        [Fact]
        public void Player_BirthDateIsLessThanMinimalAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.BirthYear = (Constants.Player.MIN_BIRTH_YEAR - 1); };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's birth year should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_BirthDateIsGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.BirthYear = Constants.Player.MAX_BIRTH_YEAR + 1; };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's birth year should not be greater than maximum allowed value.");
        }

        #endregion

        #region Height

        [Fact]
        public void Player_HeightIsLessThanMinimalAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = (Constants.Player.MIN_HEIGHT - 1); };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's height should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_HeightIsGreaterThanMaximumAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = Constants.Player.MAX_HEIGHT + 1; };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's height should not be greater than maximum allowed value.");
        }

        #endregion

        #region Weight

        [Fact]
        public void Player_WeightIsLessThanMinimalAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Weight = (Constants.Player.MIN_WEIGHT - 1); };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's weight should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_WeightIsGreaterThanMaximumAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = Constants.Player.MAX_WEIGHT + 1; };

            // Arrange
            act.Should().Throw<EntityInvariantViolationException>("Player's weight should not be greater than maximum allowed value.");
        }

        #endregion

        #region Private

        private static Player CreatePlayer()
        {
            return new Player(TEST_PLAYER_ID,
                              TEST_PLAYER_FIRST_NAME,
                              TEST_PLAYER_LAST_NAME,
                              TEST_PLAYER_BIRTH_YEAR,
                              TEST_PLAYER_HEIGHT,
                              TEST_PLAYER_WEIGHT);
        }

        private string GeneratePlayerName(int length)
        {
            return string.Concat(System.Linq.Enumerable.Repeat("a", length));
        }

        private void AssertCorrectPlayerCreated(Player actual)
        {
            actual.Id.Should().Be(TEST_PLAYER_ID, "Player's id wasn't set properly.");
            actual.FirstName.Should().Be(TEST_PLAYER_FIRST_NAME, "Player's first name wasn't set properly.");
            actual.LastName.Should().Be(TEST_PLAYER_LAST_NAME, "Player's last name wasn't set properly.");
            actual.BirthYear.Should().Be(TEST_PLAYER_BIRTH_YEAR, "Player's birth year wasn't set properly.");
            actual.Height.Should().Be(TEST_PLAYER_HEIGHT, "Player's height wasn't set properly.");
            actual.Weight.Should().Be(TEST_PLAYER_WEIGHT, "Player's weight wasn't set properly.");
        }

        #endregion
    }
}