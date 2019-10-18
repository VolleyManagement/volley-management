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

        public AzureCloudIdentityAndAccessFixture(Container container)
        {
            Container = container;
        }

        public void RegisterScenarioDependencies(Container container)
        {
            //do nothing
        }

        public void ScenarioSetup()
        {
            //do nothing
        }

        public void ScenarioTearDown()
        {
            //do nothing
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            var repo = Container.GetInstance<IUserRepository>();

            repo.Add(user);
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            // do nothing
        }

        public void VerifyUserCreated(User user)
        {
            var repo = Container.GetInstance<IUserRepository>();

            var savedUser = repo.Get(user.Tenant, user.Id).Result;

            savedUser.Should().BeSuccessful("user should be created");
            savedUser.Value.Should().BeEquivalentTo(user, "all attributes should be saved correctly");
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            var repo = Container.GetInstance<IUserRepository>();
            var deleteTasks = new List<Task>();
            foreach (var (tenant, user) in usersToTeardown)
            {
                deleteTasks.Add(repo.Delete(tenant, user));
            }

            Task.WaitAll(deleteTasks.ToArray());
        }
    }
}