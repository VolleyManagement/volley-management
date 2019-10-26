using FluentAssertions;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class AzureCloudIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private Container Container { get; }

        private List<Tuple<TenantId, UserId>> _usersToTeardown;

        public AzureCloudIdentityAndAccessFixture(Container container)
        {
            Container = container;
        }

        public void RegisterScenarioDependencies(Container container)
        {
            //do nothing
        }

        public Task ScenarioSetup()
        {
            _usersToTeardown = new List<Tuple<TenantId, UserId>>();
            return Task.CompletedTask;
        }

        public async Task ScenarioTearDown()
        {
            await CleanUpUsers(_usersToTeardown);
        }

        public async Task ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            var repo = Container.GetInstance<IUserRepository>();

            await repo.Add(user);
            _usersToTeardown.Add(Tuple.Create(tenant, id));
        }

        public Task ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            // do nothing
            return Task.CompletedTask;
        }

        public async Task VerifyUserCreated(User user)
        {
            var repo = Container.GetInstance<IUserRepository>();

            var savedUser = await repo.Get(user.Tenant, user.Id);

            savedUser.IsRight.Should().BeTrue("user should be created");
            savedUser.IfRight(u => u.Should().BeEquivalentTo(user, "all attributes should be saved correctly"));
        }

        private async Task CleanUpUsers(IEnumerable<Tuple<TenantId, UserId>> usersToTeardown)
        {
            var repo = Container.GetInstance<IUserRepository>();
            var deleteTasks = new List<Task>();
            foreach (var (tenant, user) in usersToTeardown)
            {
                deleteTasks.Add(repo.Delete(tenant, user));
            }

            await Task.WhenAll(deleteTasks.ToArray());
        }
    }
}