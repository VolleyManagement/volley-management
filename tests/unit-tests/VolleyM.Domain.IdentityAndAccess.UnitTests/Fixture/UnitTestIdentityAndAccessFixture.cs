using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    internal class UnitTestIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private readonly DomainPipelineFixtureBase _baseFixture;
        private IUserRepository _repositoryMock;
        private User _actualUser;

        public UnitTestIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public void Setup()
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            _baseFixture.Register(() => _repositoryMock, Lifestyle.Scoped);
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            _repositoryMock.Get(tenant, id).Returns(user);
            _repositoryMock.Add(Arg.Any<User>()).Returns(Error.Conflict());
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            _repositoryMock.Get(tenant, id).Returns(Error.NotFound());
            _repositoryMock.Add(Arg.Any<User>())
                .Returns(ci => ci.Arg<User>())
                .AndDoes(ci => { _actualUser = ci.Arg<User>(); });
        }

        public void VerifyUserCreated(User user)
        {
            _actualUser.Should().BeEquivalentTo(user, "all user parameters should be stored");
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            // do nothing
        }

        public void OneTimeSetup(IConfiguration configuration)
        {
            // nothing to do
        }

        public void OneTimeTearDown()
        {
            // nothing to do
        }
    }
}