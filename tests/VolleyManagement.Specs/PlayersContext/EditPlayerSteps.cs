using System;
using AutoMapper;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Exceptions;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    [Scope(Feature = "Edit Player")]
    public class EditPlayerSteps
    {
        private readonly PlayerEntity _player;
        private readonly IPlayerService _playerService;
        private Exception _exception;

        public EditPlayerSteps()
        {
            _player = new PlayerEntity();
            _playerService = IocProvider.Get<IPlayerService>();
        }

        [Given(@"(.*) player exists")]
        public void GivenPlayerExists(string fullPlayerName)
        {
            var whitespaceCharIndex = fullPlayerName.IndexOf(' ');
            var firstName = fullPlayerName.Substring(0, whitespaceCharIndex);
            var lastName = fullPlayerName.Substring(whitespaceCharIndex + 1);

            _player.FirstName = firstName;
            _player.LastName = lastName;

            TestDbAdapter.CreatePlayer(_player);
        }

        [Given(@"first name changed to (.*)")]
        public void GivenFirstNameChangedTo(string firstName)
        {
            _player.FirstName = firstName;
        }

        [Given(@"first name changed to Looong name which should be more than (.*) symbols")]
        public void GivenFirstNameChangedToNameWhichShouldBeMoreThan(int nameLength)
        {
            var name = new string('n', nameLength + 1);
            _player.FirstName = name;
        }

        [Given(@"(.*) player does not exist")]
        public void GivenPlayerDoesNotExist(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);
            _player.FirstName = firstName;
            _player.LastName = lastName;
        }

        [When(@"I execute EditPlayer")]
        public void WhenIExecuteEditPlayer()
        {
            try
            {
                _playerService.Edit(Mapper.Map<Player>(_player));
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
        }

        [Then(@"player is saved with new name")]
        public void ThenPlayerIsSavedWithNewName()
        {
            var createdPlayer = _playerService.Get(_player.Id);
            _player.Should().NotBeSameAs(createdPlayer);
        }

        [Then(@"ArgumentException is thrown")]
        public void ThenEntityInvariantViolationExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(ArgumentException), "Should thrown ArgumentException");
        }

        [Then(@"MissingEntityException is thrown")]
        public void ThenMissingEntityExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(MissingEntityException), "Should thrown MissingEntityException");
        }
    }
}