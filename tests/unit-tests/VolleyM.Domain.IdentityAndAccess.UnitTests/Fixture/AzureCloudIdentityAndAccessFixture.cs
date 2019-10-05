using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Serilog;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class AzureCloudIdentityAndAccessFixture : IIdentityAndAccessFixture
    {
        private readonly DomainPipelineFixtureBase _baseFixture;

        private TableConfiguration _tableConfig;
        private IdentityContextTableStorageOptions _options;

        public AzureCloudIdentityAndAccessFixture(DomainPipelineFixtureBase baseFixture)
        {
            _baseFixture = baseFixture;
        }

        public void Setup()
        {
            //_baseFixture.Register(() => _options, Lifestyle.Singleton);
        }

        public void ConfigureUserExists(TenantId tenant, UserId id, User user)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();

            repo.Add(user);
        }

        public void ConfigureUserDoesNotExist(TenantId tenant, UserId id)
        {
            // do nothing
        }

        public void VerifyUserCreated(User user)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();

            var savedUser = repo.Get(user.Tenant, user.Id).Result;

            savedUser.Should().BeSuccessful("user should be created");
            savedUser.Value.Should().BeEquivalentTo(user, "all attributes should be saved correctly");
        }

        public void CleanUpUsers(List<Tuple<TenantId, UserId>> usersToTeardown)
        {
            var repo = _baseFixture.Resolve<IUserRepository>();
            var deleteTasks = new List<Task>();
            foreach (var (tenant, user) in usersToTeardown)
            {
                deleteTasks.Add(repo.Delete(tenant, user));
            }

            Task.WaitAll(deleteTasks.ToArray());
        }

        public void OneTimeSetup(IConfiguration configuration)
        {
            _options = configuration.GetSection("IdentityContextTableStorageOptions")
                .Get<IdentityContextTableStorageOptions>();

            _tableConfig = new TableConfiguration(_options);
            var result = _tableConfig.ConfigureTables().Result;
            Log.Debug("AzureStorage tests using connection: {AccountName}", _tableConfig.AccountName);

            result.Should().BeSuccessful("Azure Storage should be configured correctly");
        }

        public void OneTimeTearDown()
        {
            var result = _tableConfig.CleanTables().Result;
            result.Should().BeSuccessful("Azure Storage should be cleaned up correctly");
        }
    }
}