using BoDi;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.Players.AzureStorage;

namespace VolleyM.Domain.Players.UnitTests
{
	[Binding]
	public class PlayersTestSetup : DomainTestSetupBase
	{
		// define ctor to satisfy base
		public PlayersTestSetup(IObjectContainer objectContainer) : base(objectContainer)
		{
		}

		#region One Time Fixture Setup

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			TestRunFixtureBase.OneTimeFixtureCreator = CreateOneTimeTestFixture;
			TestRunFixtureBase.BeforeTestRun();
		}

		[AfterTestRun]
		public static void AfterTestRun()
		{
			TestRunFixtureBase.AfterTestRun();
		}

		private static IOneTimeTestFixture CreateOneTimeTestFixture(TestTarget target)
		{
			return target switch
			{
				TestTarget.Unit => NoOpOneTimeTestFixture.Instance,
				TestTarget.AzureCloud => new AzureCloudPlayersOneTimeFixture(),
				TestTarget.OnPremSql => throw new NotSupportedException(),
				_ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
			};
		}

		#endregion

		protected override ITestFixture CreateTestFixture(TestTarget target)
		{
			return target switch
			{
				TestTarget.Unit => new UnitPlayersTestFixture(),
				TestTarget.AzureCloud => new AzureCloudPlayersTestFixture(Container),
				_ => throw new NotSupportedException()
			};
		}

		protected override bool RequiresAuthorizationFixture => true;

		protected override Type GetConcreteTestFixtureType => typeof(IPlayersTestFixture);

		protected override IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget target)
		{
			var result = new List<IAssemblyBootstrapper> { new DomainPlayersAssemblyBootstrapper() };

			if (target == TestTarget.AzureCloud)
			{
				result.Add(new InfrastructurePlayersAzureStorageBootstrapper());
			}

			return result;
		}
	}
}
