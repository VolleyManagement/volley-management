using System.Linq;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public abstract class PlayersTestFixtureBase : TenantTestFixtureBase
	{
		private readonly IRandomIdGenerator _idGenerator;

		protected PlayersTestFixtureBase()
		{
			_idGenerator = Substitute.For<IRandomIdGenerator>();
		}

		public override void RegisterScenarioDependencies(Container container)
		{
			base.RegisterScenarioDependencies(container);

			container.RegisterInstance(_idGenerator);
		}

		public void MockNextRandomId(string id)
		{
			_idGenerator.GetRandomId().Returns(id);
		}

		public void SetupPlayerName(IPlayerNameRequest request)
		{
			request.FirstName = SetNameField(request.FirstName);
			request.LastName = SetNameField(request.LastName);
		}

		private static string SetNameField(string val)
		{
			if (val == "<60+ symbols name>")
			{
				return new string(Enumerable.Repeat('a', 61).ToArray());
			}
			if (val == "<null>")
			{
				return null;
			}

			return val;
		}
	}
}