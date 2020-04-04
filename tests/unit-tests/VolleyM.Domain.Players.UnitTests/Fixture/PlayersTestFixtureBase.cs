using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class PlayersTestFixtureBase
	{
		protected readonly Container _container;
		private readonly IRandomIdGenerator _idGenerator;

		protected PlayersTestFixtureBase(Container container)
		{
			_container = container;
			_idGenerator = Substitute.For<IRandomIdGenerator>();
		}

		public virtual void RegisterScenarioDependencies(Container container)
		{
			container.RegisterInstance(_idGenerator);
		}

		public void MockNextRandomId(string id)
		{
			_idGenerator.GetRandomId().Returns(id);
		}
	}
}