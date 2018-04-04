using System;
using FluentAssertions;
using VolleyManagement.Domain.PlayersAggregate;
using Xunit;

namespace VolleyManagement.Domain.UnitTests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_ValidFields_ExceptionIsNowThrown()
        {
            // Arrange
            Player player;

            // Act
            Action act = () => { player = CreatePlayer(); };

            // Assert
            act.Should().NotThrow<ArgumentException>("Valid data should not throw an arguement exception.");
        }

        [Fact]
        public void Player_BirthDateHeightWeightAreNull_ExceptionIsNotThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action actBirthYear = () => { player.BirthYear = null; };
            Action actHeight = () => { player.Height = null; };
            Action actWeigth = () => { player.Weight = null; };

            // Assert
            actBirthYear.Should().NotThrow<ArgumentException>("Player's birth date can be null.");
            actHeight.Should().NotThrow<ArgumentException>("Player's height can be null.");
            actWeigth.Should().NotThrow<ArgumentException>("Player's weight can be null.");
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
            act.Should().Throw<ArgumentException>("Empty first name is not allowed.");
        }

        [Fact]
        public void Player_FirstNameContainsNumber_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = "Player1"; };

            // Assert
            act.Should().Throw<ArgumentException>("First name cannot contain number.");
        }

        [Fact]
        public void Player_FirstNameLengthGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerName(Constants.Player.MAX_FIRST_NAME_LENGTH + 1); };

            // Assert
            act.Should().Throw<ArgumentException>("First name length should not be longer than maximum allowed value.");
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
            act.Should().Throw<ArgumentException>("Empty last name is not allowed.");
        }

        [Fact]
        public void Player_LastNameContainsNumber_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.LastName = "Player1"; };

            // Assert
            act.Should().Throw<ArgumentException>("Last name cannot contain number.");
        }

        [Fact]
        public void Player_LastNameLengthGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerName(Constants.Player.MAX_LAST_NAME_LENGTH + 1); };

            // Assert
            act.Should().Throw<ArgumentException>("Last name length should not be longer than maximum allowed value.");
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
            act.Should().Throw<ArgumentException>("Player's birth year should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_BirthDateIsGreaterThanMaximumAllowedValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.BirthYear = Constants.Player.MAX_BIRTH_YEAR + 1; };

            // Arrange
            act.Should().Throw<ArgumentException>("Player's birth year should not be greater than maximum allowed value.");
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
            act.Should().Throw<ArgumentException>("Player's height should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_HeightIsGreaterThanMaximumAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = Constants.Player.MAX_HEIGHT + 1; };

            // Arrange
            act.Should().Throw<ArgumentException>("Player's height should not be greater than maximum allowed value.");
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
            act.Should().Throw<ArgumentException>("Player's weight should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_WeightIsGreaterThanMaximumAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.Height = Constants.Player.MAX_WEIGHT + 1; };

            // Arrange
            act.Should().Throw<ArgumentException>("Player's weight should not be greater than maximum allowed value.");
        }

        #endregion

        #region Private

        private static Player CreatePlayer()
        {
            return new Player {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                BirthYear = 1988,
                Height = 100,
                Weight = 80
            };
        }

        private string GeneratePlayerName(int length)
        {
            return string.Concat(System.Linq.Enumerable.Repeat("a", length));
        }

        #endregion
    }
}