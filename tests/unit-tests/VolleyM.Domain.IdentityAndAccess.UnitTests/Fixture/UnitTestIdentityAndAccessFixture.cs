using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    internal class UnitTestIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private IUserRepository _repositoryMock;
        private User _actualUser;

        public void RegisterScenarioDependencies(Container container)
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            container.Register(() => _repositoryMock, Lifestyle.Scoped);
        }

        public Task ScenarioSetup()
        {
            // do nothing
            return Task.CompletedTask;
        }

        public Task ScenarioTearDown()
        {
            // do nothing
            return Task.CompletedTask;
        }

        public Task ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            _repositoryMock.Get(tenant, id).Returns(user);
            _repositoryMock.Add(Arg.Any<User>()).Returns(Error.Conflict());

            return Task.CompletedTask;
        }

        public Task ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            _repositoryMock.Get(tenant, id).Returns(Error.NotFound());
            _repositoryMock.Add(Arg.Any<User>())
                .Returns(ci => ci.Arg<User>())
                .AndDoes(ci => { _actualUser = ci.Arg<User>(); });

            return Task.CompletedTask;
        }

        public Task VerifyUserCreated(User user)
        {
            _actualUser.Should().BeEquivalentTo(user, "all user parameters should be stored");

            return Task.CompletedTask;
        }
    }
}