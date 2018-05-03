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
using VolleyManagement.UnitTests.Services.PlayerService;
using Player = VolleyManagement.Domain.PlayersAggregate.Player;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    [Scope(Feature = "Create Player")]
    public class CreatePlayerSteps
    {
        private Player _player;
        private List<Player> _playersForBulk;
        private List<Player> _createdPlayersBulk;
        private readonly IPlayerService _playerService;
        private Exception _exception;
        private Exception _playerValidationException;
        private bool _playerValidationThrowException;

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

        [Given(@"first name set to Very looong name which should be more than (.*) symbols")]
        public void GivenFirstNameChangedToNameWhichShouldBeMoreThan(int firstNameLength)
        {
            var newFirstName = new string('n', firstNameLength + 1);
            try
            {
                _player.LastName = newFirstName;
            }
            catch (Exception exception)
            {
                _playerValidationThrowException = true;
                _playerValidationException = exception;
            }
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
        public void ThenNewPlayerGetsNewId()
        {
            Assert.NotEqual(default(int), _player.Id);
            Assert.NotEqual(int.MaxValue, _player.Id);
        }

        [Then(@"new player should be succesfully added")]
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
        public void ThenArgumentExceptionIsThrown()
        {
            if (_playerValidationThrowException)
            {
                _playerValidationException.Should().BeOfType(typeof(ArgumentException));
            }
            else
            {
                _exception.Should().BeOfType(typeof(ArgumentException));
            }
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
        public void ThenPlayerIsCreatedWithAnd(string firstName, string lastName)
        {
            PlayerEntity playerFromDb;
            using (var context = TestDbAdapter.Context)
            {
                playerFromDb = context.Players.Find(_player.Id);
            }

            playerFromDb.FirstName.Should().BeEquivalentTo(firstName);
            playerFromDb.LastName.Should().BeEquivalentTo(lastName);
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
            var playerComparer = new PlayerComparer();
            var playersInDb = new List<Player>();
            using (var context = TestDbAdapter.Context)
            {
                var length = _createdPlayersBulk.Count;
                for (var i = 0; i < length; i++)
                {
                    playersInDb.Add(Mapper.Map<Player>(context.Players.Find(_createdPlayersBulk[i].Id)));
                }
            }

            playersInDb.Count.Should().Be(_createdPlayersBulk.Count);
            var unitedCollection = playersInDb.Zip(_createdPlayersBulk, (e, a) => new { Actual = e, Expected = a });
            foreach (var playerPair in unitedCollection)
            {
                playerComparer.AreEqual(playerPair.Actual, playerPair.Expected).Should().BeTrue();
            }
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
