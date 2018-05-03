using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class RemovePlayerSteps
    {
        int _playerId;
        private readonly IPlayerService _playerService;
        private Exception _exception;

        private const int ID_PLAYER_DOES_NOT_EXIST = 1;
        public RemovePlayerSteps()
        {
            _playerService = IocProvider.Get<IPlayerService>();
        }
        [Given(@"(.*) player exists")]
        public void GivenPlayerExists(string fullPlayerName)
        {
            var whitespaceCharIndex = fullPlayerName.IndexOf(' ');
            var firstName = fullPlayerName.Substring(0, whitespaceCharIndex);
            var lastName = fullPlayerName.Substring(whitespaceCharIndex + 1);

            var player = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName
            };

            TestDbAdapter.CreatePlayer(player);
            _playerId = player.Id;
        }

        [When(@"I execute DeletePlayer")]
        public void WhenIExecuteDeletePlayer()
        {
            try
            {
                _playerService.Delete(_playerId);
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"player is removed")]
        public void ThenPlayerIsRemoved()
        {
            PlayerEntity deletedPlayer;
            using (var ctx = TestDbAdapter.Context)
            {
                deletedPlayer = ctx.Players.SingleOrDefault(p => p.Id == _playerId);
            }

            deletedPlayer.Should().Be(null, "Player should be deleted");
        }

        [Given(@"(.*) player does not exist")]
        public void GivenPlayerDoesNotExist(string fullPlayerName)
        {
            _playerId = ID_PLAYER_DOES_NOT_EXIST;
        }

        [Then(@"ConcurrencyException is thrown")]
        public void ThenConcurrencyExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(DbUpdateConcurrencyException));
        }

    }
}
