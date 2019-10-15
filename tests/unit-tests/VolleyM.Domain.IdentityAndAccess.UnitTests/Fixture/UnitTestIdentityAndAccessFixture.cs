using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System;
using System.Collections.Generic;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    internal class UnitTestIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private IUserRepository _repositoryMock;
        private User _actualUser;

        private Container Container { get; }

        public UnitTestIdentityAndAccessFixture(Container container)
        {
            Container = container;
        }

        public void Setup()
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            Container.Register(() => _repositoryMock, Lifestyle.Scoped);
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
    }
}