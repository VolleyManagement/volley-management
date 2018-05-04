using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
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
        private string _playerFirstName = "FirstName";
        private string _playerLastName = "LastName";
        private List<Player> _playersForQuickCreate;
        private List<Player> _playersAddedQuickCreateActual;
        private readonly IPlayerService _playerService;
        private Exception _exception;
        private Exception _playerValidationException;
        private bool _playerValidationThrowException;

        public CreatePlayerSteps()
        {

            _player = new Player(int.MaxValue, "First", "Last");
            _playersForQuickCreate = new List<Player>();
            _playersAddedQuickCreateActual = new List<Player>();
            _playerService = IocProvider.Get<IPlayerService>();
        }

        [Given(@"first name is (.*)")]
        public void GivenFirstNameIs(string firstName)
        {
            _playerFirstName = firstName;
        }

        [Given(@"first name set to Very looong name which should be more than (.*) symbols")]
        public void GivenFirstNameChangedToNameWhichShouldBeMoreThan(int firstNameLength)
        {
            _playerFirstName = new string('n', firstNameLength + 1);
        }

        [Given(@"last name is (.*)")]
        public void GivenLastNameIs(string lastName)
        {
            _playerLastName = lastName;
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
            _playerFirstName = null;
            _playerLastName = null;
        }

        [When(@"I execute CreatePlayer")]
        public void WhenIExecuteCreatePlayer()
        {
            try
            {
                _player.FirstName = _playerFirstName;
                _player.LastName = _playerLastName;
                var playerToAdd = Mapper.Map<CreatePlayerDto>(_player);

                _player = _playerService.Create(playerToAdd);
            }
            catch (EntityInvariantViolationException exception)
            {
                _playerValidationThrowException = true;
                _playerValidationException = exception;
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

        [Then(@"EntityInvariantViolationException is thrown")]
        public void ThenEntityInvariantViolationExceptionIsThrown()
        {
            if (_playerValidationThrowException)
            {
                _playerValidationException.Should().BeOfType(typeof(EntityInvariantViolationException));
            }
            else
            {
                _exception.Should().BeOfType(typeof(EntityInvariantViolationException));
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
            List<PlayerEntity> playersEntities;
            using (var context = TestDbAdapter.Context)
            {
                playersEntities = context.Players.Select(p => p)
                                                 .AsEnumerable()
                                                 .Where(x => _createdPlayersBulk
                                                 .Any(c => c.Id == x.Id))
                                                 .ToList();
            }
            foreach (var playerEntity in playersEntities)
            {
                playersInDb.Add(Mapper.Map<Player>(playerEntity));
            }
            playersInDb.Count.Should().Be(_createdPlayersBulk.Count);
            var unitedCollection = playersInDb.Zip(_createdPlayersBulk, (e, a) => new { Actual = e, Expected = a });
            foreach (var playerPair in unitedCollection)
            {
                playerComparer.AreEqual(playerPair.Actual, playerPair.Expected).Should().BeTrue();
            }
        }

        [Given(@"full name from Table")]
        public void GivenFullNameFromTableIs(Table table)
        {
            foreach (var row in table.Rows)
            {
                var fullName = row.Values.First();
                var whitespaceCharIndex = fullName.IndexOf(' ');
                var firstName = fullName.Substring(0, whitespaceCharIndex);
                var lastName = fullName.Substring(whitespaceCharIndex + 1);
                _playersForQuickCreate.Add(new Player(int.MaxValue, firstName, lastName));
            }
        }

        [Then(@"players is created from Table with FirstName and LastName")]
        public void ThenPlayerIsCreatedFromTableWithAnd(Table table)
        {
            if (_playersAddedQuickCreateActual.Count == table.RowCount)
            {
                for (var i = 0; i < table.RowCount; i++)
                {
                    var rowValue = table.Rows[i].Values.ToList();
                    _playersAddedQuickCreateActual[i].FirstName.Should()
                        .BeEquivalentTo(rowValue[0]);
                    _playersAddedQuickCreateActual[i].LastName.Should()
                        .BeEquivalentTo(rowValue[1]);
                }
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
