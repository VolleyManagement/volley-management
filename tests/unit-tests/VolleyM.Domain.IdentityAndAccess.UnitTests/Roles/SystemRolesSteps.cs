using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Hardcoded;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests
{
	[Binding]
	[Scope(Feature = "System Roles")]
	public class SystemRolesSteps
	{
		private RoleId _roleId;

		private Either<Error, Role> _roleResult;
		private Role _role;

		private readonly Container _container;

		public SystemRolesSteps(Container container)
		{
			_container = container;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_INIT_CONTAINER_ORDER)]
		public void ScenarioSetup()
		{
			_container.Register<IRolesStore, HardcodedRolesStore>();
		}

		[Given(@"I have Visitor role")]
		public void GivenIHaveVisitorRole()
		{
			_roleId = new RoleId("visitor");
		}

		[Given(@"I have SysAdmin role")]
		public void GivenIHaveSysAdminRole()
		{
			_roleId = new RoleId("sysadmin");
		}


		[When(@"I request role from the store")]
		public async Task WhenIRequestRoleFromTheStore()
		{
			var store = _container.GetInstance<IRolesStore>();
			_roleResult = await store.Get(_roleId);
		}

		[Then(@"role should be found")]
		public void ThenRoleShouldBeFound()
		{
			_roleResult.IsRight.Should().BeTrue("role should be found");
		}

		[Then(@"I have following permissions")]
		public void ThenIHaveFollowingPermissions(Table table)
		{
			_role = _roleResult.Match(Right: r => r, Left: l => (Role)null);

			foreach (var row in table.Rows)
			{
				var perm = new Permission(row["Context"], row["Action"]);
				_role.HasPermission(perm).Should().BeTrue($"{perm} should be allowed");
			}
		}

	}
}