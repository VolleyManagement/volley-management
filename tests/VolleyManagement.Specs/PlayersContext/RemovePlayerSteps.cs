using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.Exceptions;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class RemovePlayerSteps
    {
        private Player _player;
        private readonly IPlayerService _playerService;
        private Exception _exception;
        public RemovePlayerSteps()
        {
            _player = new Player(int.MaxValue, "CTOR", "CTOR");
            _playerService = IocProvider.Get<IPlayerService>();
        }
        [Given(@"(.*) player exists")]
        public void GivenJohnSmithPlayerExists(string fullPlayerName)
        {
            var whitespaceCharIndex = fullPlayerName.IndexOf(' ');
            var firstName = fullPlayerName.Substring(0, whitespaceCharIndex);
            var lastName = fullPlayerName.Substring(whitespaceCharIndex + 1);
            _player.FirstName = firstName;
            _player.LastName = lastName;

            _playerService.Create(AutoMapper.Mapper.Map<CreatePlayerDto>(_player));
        }

        [When(@"I execute DeletePlayer")]
        public void WhenIExecuteDeletePlayer()
        {
            try
            {
                _playerService.Delete(_player.Id);
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
                deletedPlayer = ctx.Players.SingleOrDefault(p => p.Id == _player.Id);
            }

            deletedPlayer.Should().Be(null, "Player was deleted");
        }

        [Given(@"(.*) player does not exist")]
        public void GivenIvanIvanovPlayerDoesNotExist(string fullPlayerName)
        {
            var whitespaceCharIndex = fullPlayerName.IndexOf(' ');
            var firstName = fullPlayerName.Substring(0, whitespaceCharIndex);
            var lastName = fullPlayerName.Substring(whitespaceCharIndex + 1);
            _player.FirstName = firstName;
            _player.LastName = lastName;
        }

        [Then(@"ConcurrencyException is thrown")]
        public void ThenConcurrencyExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(DbUpdateConcurrencyException));
        }

    }
}
