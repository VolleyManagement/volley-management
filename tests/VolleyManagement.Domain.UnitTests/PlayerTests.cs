using System;
using FluentAssertions;
using VolleyManagement.Domain.PlayersAggregate;
using Xunit;

namespace VolleyManagement.Domain.UnitTests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_BirthDateHeightWeightAreNull_ExceptionIsNotThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () =>
            {
                player.BirthYear = null;
                player.Height = null;
                player.Weight = null;
            };

            // Assert
            act.Should().NotThrow<ArgumentException>("Player's birth date, height and weight can be null.");
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
        public void Player_FirstNameDoesNotCorrespondsRegex_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = "Player1"; };

            // Assert
            act.Should().Throw<ArgumentException>("First name must correspond to regex.");
        }

        [Fact]
        public void Player_FirstNameLengthLongerThan60_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerNameLargerThen60(); };

            // Assert
            act.Should().Throw<ArgumentException>("First name length should not be longer than 60.");
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
        public void Player_LastNameDoesNotCorrespondsRegex_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.LastName = "Player1"; };

            // Assert
            act.Should().Throw<ArgumentException>("Last name must correspond to regex.");
        }

        [Fact]
        public void Player_LastNameLengthLongerThan60_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.FirstName = GeneratePlayerNameLargerThen60(); };

            // Assert
            act.Should().Throw<ArgumentException>("Last name length should not be longer than 60.");
        }

        #endregion

        #region BirthYear

        [Fact]
        public void Player_BirthDateIsLessThanMinimalAllowableValue_ExceptionIsThrown()
        {
            // Arrange
            var player = CreatePlayer();

            // Act
            Action act = () => { player.BirthYear = (Constants.Player.MIN_BIRTH_YEAR - 1); };

            // Arrange
            act.Should().Throw<ArgumentException>("Player's birth year should not be less than minimum allowed value.");
        }

        [Fact]
        public void Player_BirthDateIsGreaterThanMaximumAllowableValue_ExceptionIsThrown()
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
        public void Player_HeightIsLessThanMinimalAllowableValue_ExceptionIsThrown()
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
            return new Player(1, "Ivan", "Ivanov", 1988, 100, 80);
        }

        private string GeneratePlayerNameLargerThen60()
        {
            var stringBuilder = new System.Text.StringBuilder();
            for (var i = 0; i < 61; i++)
            {
                stringBuilder.Append("a");
            }
            return stringBuilder.ToString();
        }

        #endregion
    }
}