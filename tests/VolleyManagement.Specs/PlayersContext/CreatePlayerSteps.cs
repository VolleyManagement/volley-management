using AutoMapper;
using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Context;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;
using Player = VolleyManagement.Domain.PlayersAggregate.Player;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class CreatePlayerSteps
    {
        private int _addedPlayerId;
        private readonly Player _player;
        private readonly IPlayerService _playerService;

        public CreatePlayerSteps()
        {
            _player = new Player();

            _playerService = IocProvider.Get<IPlayerService>();
        }

        [Given(@"first name is (.*)")]
        public void GivenFirstNameIs(string firstName)
        {
            _player.FirstName = firstName;
        }

        [Given(@"last name is (.*)")]
        public void GivenLastNameIs(string lastName)
        {
            _player.LastName = lastName;
        }

        [Given(@"height is (.*)")]
        public void GivenHeightIs(short height)
        {
            _player.Height = height;
        }

        [Given(@"weight is (.*)")]
        public void GivenWeightIs(short weight)
        {
            _player.Weight = weight;
        }

        [Given(@"year of birth is (.*)")]
        public void GivenYearOfBirthIs(short birthYear)
        {
            _player.BirthYear = birthYear;
        }

        [Given(@"player without name")]
        public void GivenPlayerWithoutName()
        {
            _player.FirstName = null;
            _player.LastName = null;
        }

        [When(@"I execute CreatePlayer")]
        public void WhenIExecuteCreatePlayer()
        {
            var playerToAdd = Mapper.Map<Player>(_player);

            _playerService.Create(playerToAdd);

            _addedPlayerId = playerToAdd.Id;
        }

        [Then(@"new player gets new Id")]
        public void ThenNewPlayerGetsNewId()
        {
            Assert.NotEqual(default(int), _player.Id);
        }

        [Then(@"new player should be succesfully added")]
        public void ThenNewPlayerShouldBeSuccesfullyAdded()
        {
            PlayerEntity actualPlayer;
            using (var ctx = TestDbAdapter.Context)
            {
                actualPlayer = ctx.Players.SingleOrDefault(p => p.Id == _addedPlayerId);
            }

            actualPlayer.Should().NotBe(null, "Player should be saved to the DB");
            actualPlayer.Should().BeEquivalentTo(_player);
        }
    }
}
