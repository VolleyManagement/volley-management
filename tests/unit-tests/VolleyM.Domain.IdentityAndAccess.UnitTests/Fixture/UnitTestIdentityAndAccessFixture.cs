using System.Threading.Tasks;
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

        public UnitTestIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public Task Initialize()
        {
            _repositoryMock = Substitute.For<IUserRepository>();

            _baseFixture.Register(() => _repositoryMock, Lifestyle.Scoped);

            return Task.CompletedTask;
        }

        public Task ConfigureUserExists(UserId id, User user)
        {
            _repositoryMock.Get(id).Returns(user);

            return Task.CompletedTask;
        }

        public Task ConfigureUserDoesNotExist(UserId id)
        {
            _repositoryMock.Get(id).Returns(Error.NotFound());

            return Task.CompletedTask;
        }

        public Task VerifyUserCreated(User user)
        {
            _repositoryMock.Received()
                .Add(Arg.Is<User>(u => u.Id == user.Id || u.Tenant == user.Tenant));

            return Task.CompletedTask;
        }
    }
}