using System;
using FluentAssertions;
using VolleyManagement.Domain.PlayersAggregate;
using Xunit;

namespace VolleyManagement.Domain.UnitTests
{
    public class PlayerTests
    {
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

        private static Player CreatePlayer()
        {
            return new Player {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Height = 100,
                Weight = 80,
                BirthYear = 1988
            };
        }
    }
}