using System;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.Exceptions;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;
using Player = VolleyManagement.Domain.PlayersAggregate.Player;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    [Scope(Feature = "Create Player")]
    public class CreatePlayerSteps
    {
        private Player _player;
        private List<CreatePlayerDto> _playersForBulk;
        private List<Player> _createdPlayersBulk;
        private string _playerFirstName = "FirstName";
        private string _playerLastName = "LastName";
        private short? _birthYear;
        private short? _height;
        private short? _weight;
        private readonly List<string> _playersForQuickCreate;
        private readonly List<Player> _playersAddedQuickCreateActual;
        private readonly IPlayerService _playerService;
        private Exception _exception;

        public CreatePlayerSteps()
        {
            _playersForQuickCreate = new List<string>();
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
            _height = height;
        }

        [Given(@"weight is (.*)")]
        public void GivenWeightIs(short weight)
        {
            _weight = weight;
        }

        [Given(@"year of birth is (.*)")]
        public void GivenYearOfBirthIs(short birthYear)
        {
            _birthYear = birthYear;
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
            var playerToAdd = new CreatePlayerDto {
                FirstName = _playerFirstName,
                LastName = _playerLastName,
                BirthYear = _birthYear,
                Height = _height,
                Weight = _weight
            };
            try
            {
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

        [Then(@"InvalidEntityException is thrown")]
        public void ThenInvalidEntityExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(InvalidEntityException));
        }

        [When(@"I execute QuickCreatePlayer")]
        public void WhenIExecuteQuickCreatePlayer()
        {
            try
            {
                _playersAddedQuickCreateActual.AddRange(_playerService.CreateBulk(_playersForQuickCreate));
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

            playerFromDb.FirstName.Should().BeEquivalentTo(firstName, $"Firstname should be eqivalent to {firstName}!");
            playerFromDb.LastName.Should().BeEquivalentTo(lastName, $"Lastname should be eqivalent to {lastName}!");
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
                _createdPlayersBulk = _playerService.CreateBulk(_playersForBulk).ToList();
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"all players are created")]
        public void ThenAllPlayersAreCreated()
        {
            List<Player> playersInDb;
            using (var context = TestDbAdapter.Context)
            {
                var playerEntity = context.Players.ToList();
                playersInDb = playerEntity.Select(p => new Player(p.Id, p.FirstName, p.LastName)).ToList();
            }

            playersInDb.Should().BeEquivalentTo(_createdPlayersBulk,
                options => options.Including(x => x.FirstName).Including(x => x.LastName));
        }

        [Given(@"set full names from Table")]
        public void GivenFullNameFromTableIs(Table table)
        {
            foreach (var row in table.Rows)
            {
                var fullName = row.Values.First();
                _playersForQuickCreate.Add(fullName);
            }
        }

        [Then(@"players is created from Table with FirstName and LastName")]
        public void ThenPlayerIsCreatedFromTableWithAnd(Table table)
        {
            _playersAddedQuickCreateActual.Count.Should()
                .Be(table.RowCount, "Created players count must be equal to table count");

            for (var i = 0; i < table.RowCount; i++)
            {
                var rowValue = table.Rows[i].Values.ToList();
                _playersAddedQuickCreateActual[i].FirstName.Should()
                    .BeEquivalentTo(rowValue[0], $"Firstname should be eqivalent to {rowValue[0]}!");
                _playersAddedQuickCreateActual[i].LastName.Should()
                    .BeEquivalentTo(rowValue[1], $"Lastname should be eqivalent to {rowValue[1]}!");
            }
        }

        private static List<CreatePlayerDto> CreateListPlayers()
        {
            return new List<CreatePlayerDto> {
                new CreatePlayerDto {
                    FirstName = "FirstPlayer",
                    LastName = "LastName"
                },
                new CreatePlayerDto {
                    FirstName = "SecondPlayer",
                    LastName = "LastName"
                },
                new CreatePlayerDto {
                    FirstName = "ThirdPlayer",
                    LastName = "LastName"
                }
            };
        }
    }
}
