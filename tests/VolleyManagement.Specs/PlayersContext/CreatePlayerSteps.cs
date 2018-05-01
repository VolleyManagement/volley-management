using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;
using Player = VolleyManagement.Domain.PlayersAggregate.Player;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class CreatePlayerSteps
    {
        private Player _player;
        private List<Player> _playersForBulk;
        private List<Player> _createdPlayersBulk;
        private readonly IPlayerService _playerService;
        private Exception _exception;

        public CreatePlayerSteps()
        {
            _player = new Player(int.MaxValue, "First", "Last", null, null, null);
            _playerService = IocProvider.Get<IPlayerService>();
        }

        [Given(@"first name is (.*)")]
        public void GivenFirstNameIs(string firstName)
        {
            _player.FirstName = firstName;
        }

        [Given(@"first name is Very looong name which should be more than (.*) symbols")]
        [Scope(Feature = "Create Player")]
        public void GivenFirstNameChangedToNameWhichShouldBeMoreThan(int firstNameLength)
        {
            var newFirstName = new string('n', firstNameLength);
            try
            {
                _player.LastName = newFirstName;
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Given(@"last name is (.*)")]
        [Scope(Feature = "Create Player")]
        public void GivenLastNameIs(string lastName)
        {
            _player.LastName = lastName;
        }

        [Given(@"height is (.*)")]
        [Scope(Feature = "Create Player")]
        public void GivenHeightIs(short height)
        {
            _player.Height = height;
        }

        [Given(@"weight is (.*)")]
        [Scope(Feature = "Create Player")]
        public void GivenWeightIs(short weight)
        {
            _player.Weight = weight;
        }

        [Given(@"year of birth is (.*)")]
        [Scope(Feature = "Create Player")]
        public void GivenYearOfBirthIs(short birthYear)
        {
            _player.BirthYear = birthYear;
        }

        [Given(@"player without name")]
        [Scope(Feature = "Create Player")]
        public void GivenPlayerWithoutName()
        {
            _player.FirstName = null;
            _player.LastName = null;
        }

        [When(@"I execute CreatePlayer")]
        [Scope(Feature = "Create Player")]
        public void WhenIExecuteCreatePlayer()
        {
            try
            {
                var playerToAdd = Mapper.Map<CreatePlayerDto>(_player);

                _player = _playerService.Create(playerToAdd);
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"new player gets new Id")]
        [Scope(Feature = "Create Player")]
        public void ThenNewPlayerGetsNewId()
        {
            Assert.NotEqual(default(int), _player.Id);
            Assert.NotEqual(int.MaxValue, _player.Id);
        }

        [Then(@"new player should be succesfully added")]
        [Scope(Feature = "Create Player")]
        public void ThenNewPlayerShouldBeSuccesfullyAdded()
        {
            PlayerEntity actualPlayer;
            using (var ctx = TestDbAdapter.Context)
            {
                actualPlayer = ctx.Players.SingleOrDefault(p => p.Id == _player.Id);
            }

            actualPlayer.Should().NotBe(null, "Player should be saved to the DB");
            actualPlayer.Should().BeEquivalentTo(_player);
        }

        [Then(@"ArgumentException is thrown")]
        [Scope(Feature = "Create Player")]
        public void ThenArgumentExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(ArgumentException));
        }

        [Given(@"full name is (.*)")]
        public void GivenFullNameIs(string playerToCreate)
        {
            var whitespaceCharIndex = playerToCreate.IndexOf(' ');
            var firstName = playerToCreate.Substring(0, whitespaceCharIndex);
            var lastName = playerToCreate.Substring(whitespaceCharIndex + 1);
            _player.FirstName = firstName;
            _player.LastName = lastName;
        }

        [When(@"I execute QuickCreatePlayer")]
        public void WhenIExecuteQuickCreatePlayer()
        {
            try
            {
                var playerToAdd = Mapper.Map<CreatePlayerDto>(_player);

                _player = _playerService.Create(playerToAdd);
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"player is created with (.*) and (.*)")]
        public void ThenPlayerIsCreatedWithAnd(string p0, string p1)
        {
            PlayerEntity playerFromDb;
            using (var context = TestDbAdapter.Context)
            {
                playerFromDb = context.Players.Find(_player.Id);
            }

            playerFromDb.FirstName.Should().BeEquivalentTo(p0);
            playerFromDb.LastName.Should().BeEquivalentTo(p1);
        }

        [Given(@"collection of players to create")]
        public void GivenCollectionOfPlayersToCreate()
        {
            _playersForBulk = CreateListPlayers();
        }

        [When(@"I execute CreatePlayerBulk")]
        public void WhenIExecuteCreatePlayerBulk()
        {
            try
            {
                var playersToAdd = Mapper.Map<List<CreatePlayerDto>>(_playersForBulk);

                _createdPlayersBulk = _playerService.CreateBulk(playersToAdd).ToList();
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"all players are created")]
        public void ThenAllPlayersAreCreated()
        {
            _playersForBulk.Count.Should().Be(_createdPlayersBulk.Count);
        }

        private static List<Player> CreateListPlayers()
        {
            return new List<Player>{
                new Player(0,"FirstPlayer","LastName"),
                new Player(0,"SecondPlayer","LastName"),
                new Player(0,"ThirdPlayer","LastName")
            };
        }

    }
}
