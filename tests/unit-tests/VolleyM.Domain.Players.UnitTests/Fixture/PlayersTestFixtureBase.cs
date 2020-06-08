using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts.Crosscutting;
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
	}
}